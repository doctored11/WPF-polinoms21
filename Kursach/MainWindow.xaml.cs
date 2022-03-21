using System;//!
using System.Collections.Generic;//!
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;//!
using System.Windows.Controls;//!
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media; //!
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; //!
using System.Windows.Media.Animation; //!
using System.Threading;

namespace Kursach
{
    
    public partial class MainWindow : Window
    {
        double x, y, x1, x2, x3, x4, x5, y1, y2, y3, y4, y5;
        int Prov;
        

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Старт сцены
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public MainWindow()
        {
            InitializeComponent();
            AnimPrivet();///анимация при старте 

            grid();///основная сетка
            Coords();//второстепенная сетка
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Вызов основных событий -Кнопка старт
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            grid();///основная сетка
            Coords();
            Prov = 1;// проверка - 1-программа выполняется  0-надо поменять данные ввода
            GetXY();//берем данные пользователя ,проверяем тип (double)

            if (Prov == 1)
            {
                Clear_Grid();
                XYPoints(x, y, x1, x2, x3, x4, x5, y1, y2, y3, y4, y5);// строим точки пользователя
                AnimPointsVisibleTime();
                if (Prov == 1)
                {
                    Polinom1();//считаем все
                    Polinom2(x, y, x1, x2, x3, x4, x5, y1, y2, y3, y4, y5);//считаем 2
                    AnimGrahicDrawTo();//анимация графиков

                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Забираем данные у пользователя - обьявляем переменные и тд
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        double[] ax = new double[6];///массив для x координат
        double[] ay = new double[6];///массив для y координат

        void Mas(double x, double y, double x1, double x2, double x3, double x4, double x5, double y1, double y2, double y3, double y4, double y5) //кладем все в массив для использования в циклах
        {

            ax[0] = x;
            ax[1] = x1;
            ax[2] = x2;
            ax[3] = x3;
            ax[4] = x4;
            ax[5] = x5;

            ay[0] = y;
            ay[1] = y1;
            ay[2] = y2;
            ay[3] = y3;
            ay[4] = y4;
            ay[5] = y5;

            /////////////////////////////////////Сразу выполняем проверку на то что число поместится на корд прямой
            for (int i = 0; i < 6; i++)//для всех 6ти точек
            {
                if (ax[i] >= 10) //если х больше 9 то он не помещается в поле
                {
                    AnimError2();
                    Prov = 0;//переменная отвечающая за старт основных функций (проверка валидности)
                    tb1.Text = "ошибка";
                    tb2.Text = "введите значения от 0 до 9";
                }

                if (ay[i] >= 10)//если у больше 9 то он не помещается в поле
                {
                    AnimError2();
                    Prov = 0;
                    tb1.Text = "ошибка";
                    tb2.Text = "введите значения от 0 до 9";
                }
                if (ax[i] < 0) //оси только положительные - проверяем координаты на знак
                {
                    AnimError2();
                    Prov = 0;
                    tb1.Text = "ошибка";
                    tb2.Text = "введите значения от 0 до 9";
                }

                if (ay[i] < 0) //оси только положительные - проверяем координаты на знак
                {
                    AnimError2();
                    Prov = 0;
                    tb1.Text = "ошибка";
                    tb2.Text = "введите значения от 0 до 9";
                }

            }
        }



        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        void GetXY()/// забираем значения у пользователя (x y 6ти точек)
        {

            try
            {

                x = double.Parse(TX.Text);
                y = double.Parse(TY.Text);
                x1 = double.Parse(TX1.Text);
                y1 = double.Parse(TY1.Text);
                x2 = double.Parse(TX2.Text);
                y2 = double.Parse(TY2.Text);
                x3 = double.Parse(TX3.Text);
                y3 = double.Parse(TY3.Text);
                x4 = double.Parse(TX4.Text);
                y4 = double.Parse(TY4.Text);
                x5 = double.Parse(TX5.Text);
                y5 = double.Parse(TY5.Text);
            }
            catch (FormatException) //если ошибка данных - тип не double то выдаем ошибку ( программа не вылетает)
            {
                AnimError1();
                Prov = 0;
                tb1.Text = "ошибка";
                tb2.Text = "введите значения типа double";
            }

        }


        void Clear_Grid()/// чистим поле и нужные переменные
        {

            CanvasXY2.Children.Clear();
            CanvasXY.Children.Clear();
            
            grid();
            Coords();
            XMin = 999;
            XMax = 1;

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Функции Сеток - отрисовка на старте сцены
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //!-----отрисовка на трех плоскосях -Повторение кода ! - глянуть(3 плоскости для анимации графиков) -приоритет 3                          ----------!
        public void grid() // Рисеум сетку             (-публичный класс -  можно не возвращать данные) -основная сетка - старт сцены
        {
            for (int h = 0; h < 2; h++)//Рисуем координатную плоскость
            {
                for (int coordinateX = 0; coordinateX < CanvasXY2.Width / 50; coordinateX++)//Рисуем вертикальные линии 
                {
                    if (h == 0)
                    { //____________________________________________________________ 
                        Line HLine = new Line();//hLine=highLine-линии высокие 
                        HLine.X1 = coordinateX * 50;
                        HLine.Y1 = 0;
                        HLine.X2 = coordinateX * 50;
                        HLine.Y2 = CanvasXY.Height;
                        HLine.Stroke = Brushes.GhostWhite;//Красим линии 
                      //____________________________________________________________
                        Line HLineXY2 = new Line();//hLine=highLine-линии высокие 
                        HLineXY2.X1 = coordinateX * 50;
                        HLineXY2.Y1 = 0;
                        HLineXY2.X2 = coordinateX * 50;
                        HLineXY2.Y2 = CanvasXY2.Height;
                        HLineXY2.Stroke = Brushes.GhostWhite;//Красим линии 
                      //____________________________________________________________                      
                        Line HLineXY3 = new Line();//hLine=highLine-линии высокие 
                        HLineXY3.X1 = coordinateX * 50;
                        HLineXY3.Y1 = 0;
                        HLineXY3.X2 = coordinateX * 50;
                        HLineXY3.Y2 = CanvasXY2.Height;
                        HLineXY3.Stroke = Brushes.GhostWhite;//Красим линии 
                       //____________________________________________________________
                        if (coordinateX % 2 == 0) // проверка на целую клетку (2 клетки = 1)
                        {
                            HLine.StrokeThickness = 2;//Толщина основных линий
                            HLineXY2.StrokeThickness = 2;//Толщина основных линий
                            HLineXY3.StrokeThickness = 2;//Толщина основных линий

                        }
                        else
                        {
                            HLine.StrokeThickness = 0.5;//Толщина промежуточных линий
                            HLineXY2.StrokeThickness = 0.5;//Толщина промежуточных линий
                            HLineXY3.StrokeThickness = 0.5;//Толщина промежуточных линий
                        }

                        CanvasXY.Children.Add(HLine);//Переносим линии на convas
                        CanvasXY2.Children.Add(HLineXY2);//Переносим линии на convas
                        CanvasXY3.Children.Add(HLineXY3);//Переносим линии на convas
                    }
                }

                for (int coordinateY = 0; coordinateY < CanvasXY2.Height / 50; coordinateY++)//Рисуем горизонтальные линии с шагом = 1
                {
                    if (h == 1)
                    {//____________________________________________________________
                        Line Wline = new Line();//- широкие линии
                        Wline.X1 = 0;
                        Wline.Y1 = coordinateY * 50;
                        Wline.X2 = CanvasXY2.Width;
                        Wline.Y2 = coordinateY * 50;
                        Wline.Stroke = Brushes.GhostWhite;//Красим линии 
                     //____________________________________________________________
                        Line WlineXY2 = new Line();//- широкие линии
                        WlineXY2.X1 = 0;
                        WlineXY2.Y1 = coordinateY * 50;
                        WlineXY2.X2 = CanvasXY2.Width;
                        WlineXY2.Y2 = coordinateY * 50;
                        WlineXY2.Stroke = Brushes.GhostWhite;//Красим линии 
                     //____________________________________________________________
                        Line WlineXY3 = new Line();//- широкие линии
                        WlineXY3.X1 = 0;
                        WlineXY3.Y1 = coordinateY * 50;
                        WlineXY3.X2 = CanvasXY2.Width;
                        WlineXY3.Y2 = coordinateY * 50;
                        WlineXY3.Stroke = Brushes.GhostWhite;//Красим линии 
                     //____________________________________________________________
                        if (coordinateY % 2 == 0) // проверка на целую клетку (2 клетки = 1)
                        {
                            Wline.StrokeThickness = 2;//Толщина основных линий
                            WlineXY2.StrokeThickness = 2;
                            WlineXY3.StrokeThickness = 2;
                        }
                        else
                        {
                            Wline.StrokeThickness = 0.5;//Толщина промежуточных линий
                            WlineXY2.StrokeThickness = 0.5;//Толщина промежуточных линий
                            WlineXY3.StrokeThickness = 0.5;//Толщина промежуточных линий
                        }

                        CanvasXY.Children.Add(Wline);//Переносим линии на convas
                        CanvasXY2.Children.Add(WlineXY2);//Переносим линии на convas
                        CanvasXY3.Children.Add(WlineXY3);//Переносим линии на convas
                    }
                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        void Coords() //координатная прямая - 
        {
            Plot = new BrushPlot(CanvasXY2); // Создаем объект типа BrushPlot.- !!потом графики через него отрисовать надо!!!!!!
            Plot2 = new BrushPlot2(CanvasXY2);

           //____________________________________________________________
            Line vertL = new Line(); // Вертикальная ось.
            vertL.X1 = 0;
            vertL.Y1 = CanvasXY.Height;
            vertL.X2 = 0;
            vertL.Y2 = 0;
            vertL.Stroke = Brushes.DarkSlateBlue;//красим
            vertL.StrokeThickness = 3;
           //____________________________________________________________           
            Line vertL2 = new Line(); // Вертикальная ось.
            vertL2.X1 = 0;
            vertL2.Y1 = CanvasXY2.Height;
            vertL2.X2 = 0;
            vertL2.Y2 = 0;
            vertL2.Stroke = Brushes.DarkSlateBlue;//красим
            vertL2.StrokeThickness = 3;
           //____________________________________________________________
            Line vertL3 = new Line(); // Вертикальная ось.
            vertL3.X1 = 0;
            vertL3.Y1 = CanvasXY2.Height;
            vertL3.X2 = 0;
            vertL3.Y2 = 0;
            vertL3.Stroke = Brushes.DarkSlateBlue;//красим
            vertL3.StrokeThickness = 3;
           ////////////////////////////////////////////////////////
            CanvasXY.Children.Add(vertL);
            CanvasXY2.Children.Add(vertL2);
            CanvasXY3.Children.Add(vertL3);
           ///////////////////////////////////////////////////////
            Line horL = new Line(); // Горизонтальная ось.
            horL.X1 = 0;
            horL.X2 = CanvasXY.Width;
            horL.Y1 = CanvasXY.Height;
            horL.Y2 = CanvasXY.Height;
            horL.Stroke = Brushes.DarkSlateBlue;//узнать как менять цвет хэш кодом ex -#00cffa
            horL.StrokeThickness = 3;
           //____________________________________________________________
            Line horL2 = new Line(); // Горизонтальная ось.
            horL2.X1 = 0;
            horL2.X2 = CanvasXY2.Width;
            horL2.Y1 = CanvasXY2.Height;
            horL2.Y2 = CanvasXY2.Height;
            horL2.Stroke = Brushes.DarkSlateBlue;
            horL2.StrokeThickness = 3;
           //____________________________________________________________
            Line horL3 = new Line(); // Горизонтальная ось.
            horL3.X1 = 0;
            horL3.X2 = CanvasXY3.Width;
            horL3.Y1 = CanvasXY3.Height;
            horL3.Y2 = CanvasXY3.Height;
            horL3.Stroke = Brushes.DarkSlateBlue;
            horL3.StrokeThickness = 3;
           ////////////////////////////////////////////////////////
            CanvasXY.Children.Add(horL);
            CanvasXY2.Children.Add(horL2);
            CanvasXY3.Children.Add(horL3);
           ////////////////////////////////////////////////////////

            for (int i = 1; i < 20; i++) // Разметка на вертикальной оси.
            {
                Line a = new Line();
                Line a2 = new Line();
                Line a3 = new Line();

               //____________________________________________________________
                a.X1 = i * 25;
                a.X2 = i * 25;
                a.Y1 = CanvasXY2.Height - 25;
                a.Y2 = CanvasXY2.Height;
                a.Stroke = Brushes.DarkSlateBlue;//#00cffa
                a.StrokeThickness = 1;
               //____________________________________________________________
                a2.X1 = i * 25;
                a2.X2 = i * 25;
                a2.Y1 = CanvasXY2.Height - 25;
                a2.Y2 = CanvasXY2.Height;
                a2.Stroke = Brushes.DarkSlateBlue;//#00cffa
                a2.StrokeThickness = 1;
               //____________________________________________________________
                a3.X1 = i * 25;
                a3.X2 = i * 25;
                a3.Y1 = CanvasXY2.Height - 25;
                a3.Y2 = CanvasXY2.Height;
                a3.Stroke = Brushes.DarkSlateBlue;//#00cffa
                a3.StrokeThickness = 1;
               ////////////////////////////////////////////////////////
                CanvasXY.Children.Add(a);
                CanvasXY2.Children.Add(a2);
                CanvasXY3.Children.Add(a3);
               ////////////////////////////////////////////////////////
            }

            for (int i = 1; i < 20; i++) // Разметка на горизонтальной оси.
            {
                Line a = new Line();
                Line a2 = new Line();
                Line a3 = new Line();
               //____________________________________________________________
                a.X1 = 0;
                a.X2 = 25;
                a.Y1 = i * 25;
                a.Y2 = i * 25;
                a.Stroke = Brushes.DarkSlateBlue;
                a.StrokeThickness = 1;
               //____________________________________________________________
                a2.X1 = 0;
                a2.X2 = 25;
                a2.Y1 = i * 25;
                a2.Y2 = i * 25;
                a2.Stroke = Brushes.DarkSlateBlue;
                a2.StrokeThickness = 1;
               //____________________________________________________________
                a3.X1 = 0;
                a3.X2 = 25;
                a3.Y1 = i * 25;
                a3.Y2 = i * 25;
                a3.Stroke = Brushes.DarkSlateBlue;
                a3.StrokeThickness = 1;
               ////////////////////////////////////////////////////////
                CanvasXY.Children.Add(a);
                CanvasXY2.Children.Add(a2);
                CanvasXY3.Children.Add(a3);
               ////////////////////////////////////////////////////////
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //строим точки
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void XYPoints(double x, double y, double x1, double x2, double x3, double x4, double x5, double y1, double y2, double y3, double y4, double y5)///функция для отрисовки точек(используем по нажатию на кнопку)
        {
            Mas(x, y, x1, x2, x3, x4, x5, y1, y2, y3, y4, y5);
            if (Prov == 1)//проверка на валидность введеных данных
            {
                for (int i = 0; i < 6; i++)//Рисуем 6 точек
                {

                    int R = 4;//Радиус точки
                    Ellipse O = new Ellipse();
                    O.Fill = Brushes.DarkSlateBlue;//Красим точку 
                    O.Width = R * 2;
                    O.Height = R * 2;
                    O.Margin = new Thickness(ax[i] * 50 - R, CanvasXY.Height - ay[i] * 50 - R, 0, 0);//Задаём координаты точек 
                    O.Margin = new Thickness(ax[i] * 50 - R, CanvasXY2.Height - ay[i] * 50 - R, 0, 0);//Задаём координаты точек 
                    /////для второй плоскости - для дизайна - ?нельзя "удочерить o" второму обьекту(             -------!!!
                    //____________________________________________________________
                    Ellipse O2 = new Ellipse();
                    O2.Fill = Brushes.DarkSlateBlue;//Красим точку 
                    O2.Width = R * 2;
                    O2.Height = R * 2;
                    O2.Margin = new Thickness(ax[i] * 50 - R, CanvasXY.Height - ay[i] * 50 - R, 0, 0);//Задаём координаты точек 
                    O2.Margin = new Thickness(ax[i] * 50 - R, CanvasXY2.Height - ay[i] * 50 - R, 0, 0);//Задаём координаты точек 
                    //____________________________________________________________
                    CanvasXY.Children.Add(O);//Переносим точки на координатную плоскость
                    CanvasXY2.Children.Add(O2);//Переносим точки на координатную плоскость

                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Вычисления полиномов
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        
        double XMin = 999;
        double XMax = 1;
        void MinMax()
        {

            for (int i = 0; i < 6; i++)
            {

                if (ax[i] < XMin)
                    XMin = ax[i];
                if (ax[i] > XMax)
                    XMax = ax[i];

            }
        }


        BrushPlot Plot; // Создадим объект.
        BrushPlot2 Plot2; // Создадим объект.
        
       //____________________________________________________________
        void Polinom1()
        {
            string Va0;
            MinMax();
            List<Point> P = new List<Point>(); // Создаем список точек.
            List<Point> P2 = new List<Point>(); // Создаем список точек.
            double X, Y; // Объявляем необходимые переменные.
            X = XMin;

            double s0, s1, s2, b0, b1, a0, a1;

            // Используем метод наименьших квадратов.

            b0 = y1 + y2 + y3 + y4 + y5 + y;
            b1 = y1 * x1 + y2 * x2 + y3 * x3 + y4 * x4 + y5 * x5 + y * x;
            s0 = Math.Pow(x1, 0) + Math.Pow(x2, 0) + Math.Pow(x3, 0) + Math.Pow(x4, 0) + Math.Pow(x5, 0) + Math.Pow(x, 0);
            s1 = Math.Pow(x1, 1) + Math.Pow(x2, 1) + Math.Pow(x3, 1) + Math.Pow(x4, 1) + Math.Pow(x5, 1) + Math.Pow(x, 1);
            s2 = Math.Pow(x1, 2) + Math.Pow(x2, 2) + Math.Pow(x3, 2) + Math.Pow(x4, 2) + Math.Pow(x5, 2) + Math.Pow(x, 2);
            double D = s0 * s2 - s1 * s1;
            a0 = (b0 * s2 - s1 * b1) / D;
            a1 = (s0 * b1 - b0 * s1) / D;

            while (X <= XMax) // Обозначаем на графике точки, используя полученные из системы уравнений коэффициенты.
            {
                Y = a0 + a1 * X; // Полином первого порядка.
                P.Add(new Point(X, Y));
                X = X + 0.1;
            }
            
            if (Math.Round(a0, 2) >= 0) //проверка знака для вывода
            {
                Va0 = "+" + Math.Round(a0, 2);
            }
            else
            {
                Va0 = " " + Math.Round(a0, 2);
            }

            tb1.Text = "P1(x) = " + Math.Round(a1, 2)+ "x" + Va0; // Выводим на экран полученную функцию.
            X = 0;
            while (X <= 9) // Обозначаем на графике точки, используя полученные из системы уравнений коэффициенты.
            {
                Y = a0 + a1 * X; // Полином первого порядка.
                P2.Add(new Point(X, Y));
                X = X + 0.1;
            }
            Plot.Draw(P); // Соединяем.
            Plot2.Draw(P2); // Соединяем.

        }

       //____________________________________________________________

        void Polinom2(double x, double y, double x1, double x2, double x3, double x4, double x5, double y1, double y2, double y3, double y4, double y5)
        {
            MinMax();
            string Va0, Va1;
            List<Point> P = new List<Point>(); // Обявляем список точек.
            List<Point> P2 = new List<Point>(); // Обявляем список точек.
            double X, Y; // Объявляем необходимые переменные.
            X = XMin;

            double s0, s1, s2, s3, s4, b0, b1, b2, a0, a1, a2;
            // Используем метол наименьших квадратов.
            s0 = Math.Pow(x1, 0) + Math.Pow(x2, 0) + Math.Pow(x3, 0) + Math.Pow(x4, 0) + Math.Pow(x5, 0) + Math.Pow(x, 0);
            s1 = Math.Pow(x1, 1) + Math.Pow(x2, 1) + Math.Pow(x3, 1) + Math.Pow(x4, 1) + Math.Pow(x5, 1) + Math.Pow(x, 1);
            s2 = Math.Pow(x1, 2) + Math.Pow(x2, 2) + Math.Pow(x3, 2) + Math.Pow(x4, 2) + Math.Pow(x5, 2) + Math.Pow(x, 2);
            s3 = Math.Pow(x1, 3) + Math.Pow(x2, 3) + Math.Pow(x3, 3) + Math.Pow(x4, 3) + Math.Pow(x5, 3) + Math.Pow(x, 3);
            s4 = Math.Pow(x1, 4) + Math.Pow(x2, 4) + Math.Pow(x3, 4) + Math.Pow(x4, 4) + Math.Pow(x5, 4) + Math.Pow(x, 4);
            b0 = y1 + y2 + y3 + y4 + y5 + y;
            b1 = y1 * x1 + y2 * x2 + y3 * x3 + y4 * x4 + y5 * x5 + y * x;
            b2 = y1 * x1 * x1 + y2 * x2 * x2 + y3 * x3 * x3 + y4 * x4 * x4 + y5 * x5 * x5 + y * x * x;
            double D = s0 * s2 * s4 + s1 * s3 * s2 + s1 * s3 * s2 - s2 * s2 * s2 - s1 * s1 * s4 - s3 * s3 * s0;
            a0 = (b0 * s2 * s4 + s1 * s3 * b2 + b1 * s3 * s2 - s2 * s2 * b2 - s1 * b1 * s4 - s3 * s3 * b0) / D;
            a1 = (s0 * b1 * s4 + b0 * s3 * s2 + s1 * b2 * s2 - s2 * s2 * b1 - s1 * b0 * s4 - b2 * s3 * s0) / D;
            a2 = (s0 * s2 * b2 + s1 * b1 * s2 + s1 * s3 * b0 - s2 * s2 * b0 - s1 * b2 * s1 - s0 * s3 * b1) / D;

            while (X <= XMax) // Обозначаем на графике точки, используя полученные из системы уравнений коэффициенты.
            {
                Y = a0 + a1 * X + a2 * X * X; // Полином второго порядка.

                P.Add(new Point(X, Y));
                X = X + 0.1;
            }
            //////////
            if (Math.Round(a1, 2) >= 0) //проверка знака для вывода
            {
                Va1 = "+" + Math.Round(a1, 2);
            }
            else
            {
                Va1 = " " + Math.Round(a1, 2);
            }
            if (Math.Round(a0, 2) >= 0) //проверка знака для вывода
            {
                Va0 = "+" + Math.Round(a0, 2);
            }
            else
            {
                Va0 = " " + Math.Round(a0, 2);
            }
            /////////
            Plot.Draw(P); // Соединяем.
            tb2.Text = "P2(x) = " + Math.Round(a2, 2) + "x^2" + Va1 + "x" + Va0 ;
            // Выводим на экран полученную функцию.
            X = 0;
            while (X <= 9) // Обозначаем на графике точки, используя полученные из системы уравнений коэффициенты.
            {
                Y = a0 + a1 * X + a2 * X * X; // Полином второго порядка.
                P2.Add(new Point(X, Y));
                X = X + 0.1;
            }
            Plot2.Draw(P2); // Соединяем.
            XMin = 999;
            XMax = 1;

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Заполнение вариантов
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        void _1_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "0,0";
            TX1.Text = "0,2";
            TX2.Text = "0,4";
            TX3.Text = "0,6";
            TX4.Text = "0,8";
            TX5.Text = "1,0";
            /////////////////////////////////////////
            TY.Text = "3,0";
            TY1.Text = "6,0";
            TY2.Text = "3,0";
            TY3.Text = "6,0";
            TY4.Text = "4,0";
            TY5.Text = "3,0";

        }

        private void _2_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "0,0";
            TX1.Text = "0,2";
            TX2.Text = "0,4";
            TX3.Text = "0,6";
            TX4.Text = "0,8";
            TX5.Text = "1,0";
            /////////////////////////////////////////
            TY.Text = "5,0";
            TY1.Text = "5,0";
            TY2.Text = "4,0";
            TY3.Text = "4,0";
            TY4.Text = "6,0";
            TY5.Text = "6,0";
        }

        private void _3_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "3,0";
            TX1.Text = "3,2";
            TX2.Text = "3,4";
            TX3.Text = "3,6";
            TX4.Text = "3,8";
            TX5.Text = "4,0";
            /////////////////////////////////////////
            TY.Text = "2,0";
            TY1.Text = "3,0";
            TY2.Text = "3,0";
            TY3.Text = "3,0";
            TY4.Text = "2,0";
            TY5.Text = "4,0";
        }

        private void _4_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "3,0";
            TX1.Text = "3,2";
            TX2.Text = "3,4";
            TX3.Text = "3,6";
            TX4.Text = "3,8";
            TX5.Text = "4,0";
            /////////////////////////////////////////
            TY.Text = "6,0";
            TY1.Text = "2,0";
            TY2.Text = "6,0";
            TY3.Text = "4,0";
            TY4.Text = "3,0";
            TY5.Text = "4,0";
        }

        private void _5_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "5,0";
            TX1.Text = "5,2";
            TX2.Text = "5,4";
            TX3.Text = "5,6";
            TX4.Text = "5,8";
            TX5.Text = "6,0";
            /////////////////////////////////////////
            TY.Text = "2,0";
            TY1.Text = "4,0";
            TY2.Text = "4,0";
            TY3.Text = "3,0";
            TY4.Text = "3,0";
            TY5.Text = "3,0";
        }

        private void _6_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "4,0";
            TX1.Text = "4,2";
            TX2.Text = "4,4";
            TX3.Text = "4,6";
            TX4.Text = "4,8";
            TX5.Text = "5,0";
            /////////////////////////////////////////
            TY.Text = "4,0";
            TY1.Text = "3,0";
            TY2.Text = "6,0";
            TY3.Text = "6,0";
            TY4.Text = " 4,0";
            TY5.Text = " 4,0";
        }

        private void _7_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "1,0";
            TX1.Text = "1,2";
            TX2.Text = "1,4";
            TX3.Text = "1,6";
            TX4.Text = "1,8";
            TX5.Text = "2,0";
            /////////////////////////////////////////
            TY.Text = "2,0";
            TY1.Text = "6,0";
            TY2.Text = "4,0";
            TY3.Text = "4,0";
            TY4.Text = "2,0";
            TY5.Text = "5,0";
        }

        private void _8_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "5,0";
            TX1.Text = "5,2";
            TX2.Text = "5,4";
            TX3.Text = "5,6";
            TX4.Text = "5,8";
            TX5.Text = "6,0";
            /////////////////////////////////////////
            TY.Text = "3,0";
            TY1.Text = "2,0";
            TY2.Text = "5,0";
            TY3.Text = "2,0";
            TY4.Text = "2,0";
            TY5.Text = "3,0";
        }

        private void _9_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "2,0";
            TX1.Text = "2,2";
            TX2.Text = "2,4";
            TX3.Text = "2,6";
            TX4.Text = "2,8";
            TX5.Text = "3,0";
            /////////////////////////////////////////
            TY.Text = "4,0";
            TY1.Text = "2,0";
            TY2.Text = "4,0";
            TY3.Text = "2,0";
            TY4.Text = "5,0";
            TY5.Text = "2,0";
        }

        private void _10_Click(object sender, RoutedEventArgs e)
        {
            TX.Text = "0,0";
            TX1.Text = "0,2";
            TX2.Text = "0,4";
            TX3.Text = "0,6";
            TX4.Text = "0,8";
            TX5.Text = "1,0";
            /////////////////////////////////////////
            TY.Text = "6,0";
            TY1.Text = "3,0";
            TY2.Text = "2,0";
            TY3.Text = "6,0";
            TY4.Text = "2,0";
            TY5.Text = "5,0";
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Grid();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Анимации
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void AnimPrivet() //анимация на старт
        {
            DoubleAnimation FAnimation = new DoubleAnimation();
            FAnimation.From = 1200;
            FAnimation.To = 0;
            FAnimation.Duration = TimeSpan.FromSeconds(3);
            AnimBord.BeginAnimation(Border.HeightProperty, FAnimation);
        }


        void AnimPointsVisibleTime() //аниация отрисовки точек
        {
            DoubleAnimation VTAnimation = new DoubleAnimation();
            VTAnimation.From = 0;
            VTAnimation.To = 1;
            VTAnimation.Duration = TimeSpan.FromSeconds(0.45);
            CanvasXY.BeginAnimation(Canvas.OpacityProperty, VTAnimation);
            //____________________________________________________________
            DoubleAnimation VTAnimation2 = new DoubleAnimation();
            VTAnimation2.From = -1;
            VTAnimation2.To = 1;
            VTAnimation2.Duration = TimeSpan.FromSeconds(0.8);
            CanvasXY2.BeginAnimation(Canvas.OpacityProperty, VTAnimation2);
        }

        void AnimGrahicDrawTo()//анимация показа графика
        {
            DoubleAnimation DrawAnimation = new DoubleAnimation();
            DrawAnimation.From = 502;
            DrawAnimation.To = 0;
            DrawAnimation.Duration = TimeSpan.FromSeconds(2);
            CanvasXY.BeginAnimation(Canvas.WidthProperty, DrawAnimation);
        }

        void AnimError1()//анимация про иошибке типа данных
        {
            DoubleAnimation ErAnimation = new DoubleAnimation();
            ErAnimation.From = 503;
            ErAnimation.To = 0;
            ErAnimation.Duration = TimeSpan.FromSeconds(2);
            AniError.BeginAnimation(Border.HeightProperty, ErAnimation);
        }
        void AnimError2() //ошибка при мал и больш значениях 
        {
            DoubleAnimation ErAnimation = new DoubleAnimation();
            ErAnimation.From = 1;
            ErAnimation.To = 0;
            ErAnimation.Duration = TimeSpan.FromSeconds(3);
            AniError2.BeginAnimation(Border.OpacityProperty, ErAnimation);
        }   
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Отрисовка линий - класс
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //2 одинаковых класса- поправить - приоритет 4                                      ----!
    public class BrushPlot // Создаем класс.
    {
        Canvas CanvasXY; // Создаем холст.
        Brush br; // Объявляем кисть.
        public BrushPlot(Canvas CanvasXY)
        {
            this.CanvasXY = CanvasXY;
            br = Brushes.Purple;
            // Цвет кисти - ./
            // Цвет кисти - .//#ff0038
        }
        public void Draw(List<Point> P) // Создаем метод, который будет рисовать сам график.
        {
            PointCollection PC = new PointCollection(); // Создаем перменную, состоящую из коллекции точек.

            foreach (Point p in P) // Получаем при обходе списка координаты точек.
            {
                if (p.Y >= 0 && p.X >= 0 && p.Y <= 9) // Учитываем, что график не должен выходить за пределы холста.
                {
                    PC.Add(new Point(p.X * CanvasXY.Width / 10, CanvasXY.Height - p.Y * CanvasXY.Height / 10));
                }

            }
            Polyline PL = new Polyline(); // Соединяем точки.
            PL.Points = PC;
            PL.Stroke = br;
            PL.StrokeThickness = 3;
            CanvasXY.Children.Add(PL); // Рисуем сам график.
        }


    }
    //____________________________________________________________
    public class BrushPlot2 // Создаем класс.
    {
        Canvas CanvasXY; // Создаем холст.
        Brush  brr; // Объявляем кисть.
        public BrushPlot2(Canvas CanvasXY)
        {
            this.CanvasXY = CanvasXY;
            // br = Brushes.Purple;
            brr = Brushes.Black;// Цвет кисти - ./
                                // Цвет кисти - .//#ff0038
        }
        public void Draw(List<Point> P2) // Создаем метод, который будет рисовать сам график.
        {
            PointCollection PC = new PointCollection(); // Создаем перменную, состоящую из коллекции точек.

            foreach (Point po in P2) // Получаем при обходе списка координаты точек.
            {
                if (po.Y >= 0 && po.X >= 0 && po.Y <= 9) // Учитываем, что график не должен выходить за пределы холста.
                {
                    PC.Add(new Point(po.X * CanvasXY.Width / 10, CanvasXY.Height - po.Y * CanvasXY.Height / 10));
                }

            }
            Polyline PL = new Polyline(); // Соединяем точки.
            PL.Points = PC;
            PL.Stroke = brr;
            PL.StrokeThickness = 0.5;
            CanvasXY.Children.Add(PL); // Рисуем сам график.

        }
        //____________________________________________________________
        //____________________________________________________________
        //____________________________________________________________
        //_________________________________________________________________________________________Эосо-02-20____Шульц____________________________________________

    }
}

