using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Engine.Events;
using Engine.Extensions;
using System.IO;
using NAudio.Wave;
using System.Threading;


namespace Engine
{
    public class GameObject
    {
        public List<GameObject> StarList = new List<GameObject>();
        public bool saliDeEscena = false;


        private List<GameObject> children = new List<GameObject>();
        private GameObject parent;
        private RectangleF bounds = new RectangleF(0, 0, 30, 40);
        private EventHandler eventHandler = new EventHandler();
        private bool visible = true;//Eliminar?

        public List<GameObject> Children //Devuelve una lista de objetos
        {
            get { return children; }
        }

        public IEnumerable<GameObject> AllChildren //lo puedo modificar para que simplemente devuelva children
        {
            get
            {
                List<GameObject> result = new List<GameObject>();
                foreach (GameObject m1 in children.ToArray())
                {
                    result.Add(m1);
                    foreach (GameObject m2 in m1.AllChildren)
                    {
                        result.Add(m2);
                    }
                }
                return result;
            }
        }//devuelve una nueva lista con todos los children (Revisar)

        public IEnumerable<GameObject> AllObjects
        {
            get
            {
                return Root.AllChildren;
            }
        }

        public GameObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public GameObject Root
        {
            get { return parent != null ? parent.Root : this; }
        }

        public EventHandler EventHandler//Evento event handler
        {
            get { return eventHandler; }
        }

        public bool Visible//Guarda un bool para saber si es visible o no
        {
            get { return visible; }
            set { visible = value; }
        }

        public Bitmap Image //Propiedad devuelve una Image que manda a recortar y dibujar
        {
            get
            {
                Bitmap temp = new Bitmap((int)Width, (int)Height);
                Graphics graphics = Graphics.FromImage(temp);
                graphics.TranslateTransform(-X, -Y);
                DrawOn(graphics);
                graphics.Dispose();
                return temp;
            }
        }

        public RectangleF Bounds// propiedad de mis bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public float X //Mi posicion x
        {
            get { return Left; }
            set { Left = value; }
        }

        public float Y //Mi posicion y
        {
            get { return Top; }
            set { Top = value; }
        }

        public float Left// Mi posicion x
        {
            get { return bounds.Left; }
            set { Position = new PointF(value, Top); }
        }

        public float Right
        {
            get { return bounds.Right; }
            set { Position = new PointF(value - Width, Top); }
        }

        public float Top //Mi posicion y
        {
            get { return bounds.Top; }
            set { Position = new PointF(Left, value); }
        }

        public float Bottom
        {
            get { return bounds.Bottom; }
            set { Position = new PointF(Left, value - Height); }
        }

        public PointF Position//Mi posicion
        {
            get { return new PointF(Left, Top); }
            set { MoveDelta(value.X - X, value.Y - Y); }
        }

        public PointF Center//Mi centro del objeto
        {
            get { return new PointF(Position.X + Width / 2, Position.Y + Height / 2); }
            set { Position = new PointF(value.X - Width / 2, value.Y - Height / 2); }
        }

        public PointF TopLeft//mas posiciones
        {
            get { return Position; }
            set { Position = value; }
        }

        public PointF TopRight
        {
            get { return new PointF(Right, Top); }
            set
            {
                Right = value.X;
                Top = value.Y;
            }
        }

        public PointF BottomLeft
        {
            get { return new PointF(Left, Bottom); }
            set
            {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public PointF BottomRight
        {
            get { return new PointF(Right, Bottom); }
            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        public float CenterX
        {
            get { return Center.X; }
            set { Center = new PointF(value, Center.Y); }
        }

        public float CenterY
        {
            get { return Center.Y; }
            set { Center = new PointF(Center.X, value); }
        }

        public SizeF Extent
        {
            get { return new SizeF(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public float Width
        {
            get { return bounds.Width; }
            set { bounds = new RectangleF(bounds.Left, bounds.Top, value, bounds.Height); }
        }

        public float Height
        {
            get { return bounds.Height; }
            set { bounds = new RectangleF(bounds.Left, bounds.Top, bounds.Width, value); }
        }


        //metodos por revisar

        protected void MoveDelta(float x, float y)
        {
            bounds = new RectangleF(bounds.Left + x, bounds.Top + y, bounds.Width, bounds.Height);
            children.ForEach((m) => m.MoveDelta(x, y));
        }
        
        public void AddChild(GameObject child)// Añade objetos a la lista Child y guarda el objeto como Parent
        {
            children.Add(child);
            child.Parent = this;
        }

        public void AddStars(GameObject Star)//
        {
            StarList.Add(Star);
        }

        public void AddChildren(IEnumerable<GameObject> children)
        {
            foreach (GameObject m in children)
            {
                AddChild(m);
            }
        }

        public void RemoveChild(GameObject child)
        {
            children.Remove(child);
            child.Parent = null;
        }

        public void RemoveStar()//
        {
           var newListStar = StarList.Where((x => x.saliDeEscena == false)).ToList() ;
            StarList = newListStar;

        }


        public void Delete()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        public Color ColorAt(Point point, bool globalCoordinates = true)
        {
            if (globalCoordinates)
            {
                point = new Point(
                    point.X - Left.RoundedToInt(),
                    point.Y - Top.RoundedToInt());
            }
            try
            {
                return Image.GetPixel(point.X, point.Y);
            }
            catch(Exception)
            {
                return Color.Transparent;
            }
        }

        public IEnumerable<Point> NonTransparentPoints(bool globalCoordinates = true)
        {
            Bitmap img = Image;
            List<Point> result = new List<Point>();
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    if (img.GetPixel(x,y).A > 0)
                    {
                        Point point = new Point(x, y);
                        if (globalCoordinates)
                        {
                            point = new Point(
                                point.X + Left.RoundedToInt(),
                                point.Y + Top.RoundedToInt());
                        }
                        result.Add(point);
                    }
                }
            }
            return result;
        }

        public bool IsTransparentAt(PointF point, bool globalCoordinates = true)
        {
            return IsTransparentAt(new Point(point.X.RoundedToInt(), point.Y.RoundedToInt()));
        }

        public bool IsTransparentAt(Point point, bool globalCoordinates = true)
        {
            return ColorAt(point, globalCoordinates).A == 0;
        }

        public bool IsTranslucentAt(PointF point, bool globalCoordinates = true)
        {
            return IsTranslucentAt(new Point(point.X.RoundedToInt(), point.Y.RoundedToInt()));
        }

        public bool IsTranslucentAt(Point point, bool globalCoordinates = true)
        {
            return ColorAt(point, globalCoordinates).A < 255;
        }

        public bool IsOpaqueAt(PointF point, bool globalCoordinates = true)
        {
            return IsOpaqueAt(new Point(point.X.RoundedToInt(), point.Y.RoundedToInt()));
        }

        public bool IsOpaqueAt(Point point, bool globalCoordinates = true)
        {
            return ColorAt(point, globalCoordinates).A == 255;
        }

        public bool CollidesWith(GameObject obj)
        {
            return obj != null
                && obj.Bounds.IntersectsWith(Bounds)
                && obj.NonTransparentPoints().Intersect(NonTransparentPoints()).Any();
        }

        public virtual void DrawOn(Graphics graphics)
        {
            // Do nothing. Subclasses should override
        }

        public virtual void Update(float deltaTime)
        {
            // Do nothing. Subclasses should override
        }

        public virtual void UpdateStar(float deltaTime)
        {
            // Do nothing. Subclasses should override
        }

        internal void FullUpdate(float deltaTime, bool world = false)//La manera en la que se actualiza es muy mala, es mejor un foreach
        {
            //foreach (var chill in children)
            //{
            //    chill.Update(deltaTime);
            //}

            if (Parent == null && !world) return;
            Update(deltaTime);
            children.ToList().ForEach((m) => m.FullUpdate(deltaTime));
        }

        internal void UpdateStars(float deltaTime)
        {

            foreach (var star in StarList)
            {
                star.UpdateStar(deltaTime);
            }
            RemoveStar();
            //if (StarList != null) 
            //{
            //    StarArray.ToList().ForEach((m) => m.UpdateStar(deltaTime));
            //    RemoveStar();
            //}

            //var ListaSuplente = StarsList;
            //for (int i = 0; i < ListaSuplente.Count; i++)
            //{
            //    //Arreglar el Remove Estrellas y el codigo funciona
            //}

            
        }

        internal void FullDrawOn(Graphics graphics)
        {
            foreach (var obj in children)
            {
                obj.DrawOn(graphics);
                //obj.DrawBoundsOn(graphics);
            }

            foreach (var star in StarList)
            {
                star.DrawOn(graphics);
                //star.DrawBoundsOn(graphics);
            }

            //if (!visible) return;
            //DrawOn(graphics);
            ////DrawBoundsOn(graphics);
            //children.ForEach((m) => m.FullDrawOn(graphics));
        }

        private void DrawBoundsOn(Graphics graphics)//Dibuja la HitBox del personaje
        {
            graphics.DrawRectangle(Pens.Red, X, Y, Width, Height);
        }

        internal void HandleEvent(Event evt)
        {
            foreach (GameObject child in children.ToArray().Reverse())
            {
                child.HandleEvent(evt);
            }
            if (!evt.Handled && evt.Accepts(this))
            {
                eventHandler.TryToHandle(evt);
            }
        }
        
        public void Play(Stream soundEffect)
        {
            using (WaveFileReader reader = new WaveFileReader(soundEffect))
            {
                WaveOut wout = new WaveOut();
                wout.Init(reader);
                wout.PlaybackStopped += (sender, e) =>
                {
                    wout.Dispose();
                };
                wout.Play();
            }
        }
    }
}
