using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace Invaders
{
    public partial class MainWindow : Window
    {
        int moveSpan = 10;
        int playerLeftMargin = 100;
        Rectangle[] Bullets = new Rectangle[10];
        Ellipse[] stars = new Ellipse[100];
        int nowBullet = 0;
        float bulletSpeed = 10;
        Dictionary<string, BitmapImage[]> Images = new Dictionary<string, BitmapImage[]>();
        Image[,] EmemyArray = new Image[4,6];
        int imageCnt = 0;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();

            Setting();
           
            DispatcherTimer timer = new DispatcherTimer(); 
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += new EventHandler(timer_Tick); 
            timer.Start();

            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(500);
            timer2.Tick += new EventHandler(timer_Tick_low);
            timer2.Start();
        }


        private void Setting()
        {
            Images["bug"] = new BitmapImage[4];
            Images["bug"][0] = new BitmapImage(new Uri("Asset/bug1.png", UriKind.Relative));
            Images["bug"][1] = new BitmapImage(new Uri("Asset/bug2.png", UriKind.Relative));
            Images["bug"][2] = new BitmapImage(new Uri("Asset/bug3.png", UriKind.Relative));
            Images["bug"][3] = new BitmapImage(new Uri("Asset/bug4.png", UriKind.Relative));

            Images["satellite"] = new BitmapImage[4];
            Images["satellite"][0] = new BitmapImage(new Uri("Asset/satellite1.png", UriKind.Relative));
            Images["satellite"][1] = new BitmapImage(new Uri("Asset/satellite2.png", UriKind.Relative));
            Images["satellite"][2] = new BitmapImage(new Uri("Asset/satellite3.png", UriKind.Relative));
            Images["satellite"][3] = new BitmapImage(new Uri("Asset/satellite4.png", UriKind.Relative));

            Images["flyingsaucer"] = new BitmapImage[4];
            Images["flyingsaucer"][0] = new BitmapImage(new Uri("Asset/flyingsaucer1.png", UriKind.Relative));
            Images["flyingsaucer"][1] = new BitmapImage(new Uri("Asset/flyingsaucer2.png", UriKind.Relative));
            Images["flyingsaucer"][2] = new BitmapImage(new Uri("Asset/flyingsaucer3.png", UriKind.Relative));
            Images["flyingsaucer"][3] = new BitmapImage(new Uri("Asset/flyingsaucer4.png", UriKind.Relative));

            Images["spaceship"] = new BitmapImage[4];
            Images["spaceship"][0] = new BitmapImage(new Uri("Asset/spaceship1.png", UriKind.Relative));
            Images["spaceship"][1] = new BitmapImage(new Uri("Asset/spaceship2.png", UriKind.Relative));
            Images["spaceship"][2] = new BitmapImage(new Uri("Asset/spaceship3.png", UriKind.Relative));
            Images["spaceship"][3] = new BitmapImage(new Uri("Asset/spaceship4.png", UriKind.Relative));

            Images["star"] = new BitmapImage[4];
            Images["star"][0] = new BitmapImage(new Uri("Asset/star1.png", UriKind.Relative));
            Images["star"][1] = new BitmapImage(new Uri("Asset/star2.png", UriKind.Relative));
            Images["star"][2] = new BitmapImage(new Uri("Asset/star3.png", UriKind.Relative));
            Images["star"][3] = new BitmapImage(new Uri("Asset/star4.png", UriKind.Relative));

            Images["watchit"] = new BitmapImage[4];
            Images["watchit"][0] = new BitmapImage(new Uri("Asset/watchit1.png", UriKind.Relative));
            Images["watchit"][1] = new BitmapImage(new Uri("Asset/watchit2.png", UriKind.Relative));
            Images["watchit"][2] = new BitmapImage(new Uri("Asset/watchit3.png", UriKind.Relative));
            Images["watchit"][3] = new BitmapImage(new Uri("Asset/watchit4.png", UriKind.Relative));



            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    EmemyArray[i, j] = Enemys.FindName(string.Format("enemy{0}{1}", i, j)) as Image;
                }
            }
           
            Canvas.SetLeft(player, playerLeftMargin);
            scoreDisplay.Content = score.ToString();

            for (int i = 0; i < 10; i++)
            {
                Bullets[i] = new Rectangle();
                Bullets[i].Height = 20;
                Bullets[i].Width = 5;
                Bullets[i].Fill = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                Canvas.SetBottom(Bullets[i], -100);
                world.Children.Add(Bullets[i]);
            }

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                

                stars[i] = new Ellipse();
                int size = random.Next(1, 5);
                stars[i].Width = size;
                stars[i].Height = size;
                stars[i].Fill = new SolidColorBrush(Color.FromRgb(100,100, 0));

                Canvas.SetLeft(stars[i], random.Next(0, 570));
                Canvas.SetBottom(stars[i], random.Next(0, 560));

                world.Children.Add(stars[i]);
            }
        }
        private void player_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    if (playerLeftMargin < 470)
                        playerLeftMargin += moveSpan;
                    if (playerLeftMargin >= 470)
                        playerLeftMargin = 469;
                    break;

                case Key.Left:
                    if (playerLeftMargin > 10)
                        playerLeftMargin -= moveSpan;
                    if (playerLeftMargin < 10)
                        playerLeftMargin = 10;
                    break;

                case Key.Space:
                    Canvas.SetBottom(Bullets[nowBullet], 70);
                    Canvas.SetLeft(Bullets[nowBullet], 38 + playerLeftMargin);
                    nowBullet++;
                    if (nowBullet > 9)
                        nowBullet = 0;
                    
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Canvas.SetLeft(player, playerLeftMargin);

            for (int i = 0; i < 10; i++)
            {
                float bulletYPos = (float)Canvas.GetBottom(Bullets[i]);
                float bulletXPos = (float)Canvas.GetLeft(Bullets[i]);
                if (bulletYPos > 0)
                {  
                    Canvas.SetBottom(Bullets[i], bulletYPos + bulletSpeed);
                    if (bulletYPos > 620) 
                        Canvas.SetBottom(Bullets[i], -100);
                    else
                    {
                        if (bulletYPos > 220 && bulletYPos < 220 + 280 && bulletXPos > 70 && bulletXPos < 70 + 420)
                        {
                            int row = -1;
                            int column = -1;

                            row = 3 - (int)((bulletYPos - 220f) / 70f);
                            column = (int)((bulletXPos - 70f) / 70f);

                            Image delEnemy = Enemys.FindName(string.Format("enemy{0}{1}", row, column)) as Image;

                            if(delEnemy.Width > 0)
                            {
                                delEnemy.Width = 0;
                                Canvas.SetBottom(Bullets[i], -100);
                                score += 10;
                                scoreDisplay.Content = score.ToString();
                            }
                        }
                        /*

                        if (bulletYPos > 220 && bulletYPos < 220 + 70 && bulletXPos > 70 && bulletXPos < 70 + 70)
                        {
                            Enemys.Children.Remove(enemy30);
                        }
                        */
                    }
                }
            }
        }

        private void timer_Tick_low(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int size = random.Next(1, 5);
                stars[i].Width = size;
                stars[i].Height = size;
                //stars[i].Fill = new SolidColorBrush(Color.FromRgb(100, 100, 0));

                //Canvas.SetLeft(stars[i], random.Next(0, 570));
                //Canvas.SetBottom(stars[i], random.Next(0, 560));
            }

            for (int i = 0; i < 5; i++)
            {
                EmemyArray[0, i].Source = Images["satellite"][imageCnt];
                EmemyArray[1, i].Source = Images["bug"][imageCnt];
                EmemyArray[2, i].Source = Images["flyingsaucer"][imageCnt];
                EmemyArray[3, i].Source = Images["star"][imageCnt];
            }

            if (++imageCnt > 3)
                imageCnt = 0;
        }
    }
}
