using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIR
{
    internal class Filters
    {        
        //Сепия
        //Чёрно-Белый
        //Размытие
        //Оттенки серого
        //Резкость
        //Негатив
        //Фильтр Собела
        //Фильтр Робертса
        //Адаптивный фильтр подавления шума
        //Частотный фильтр Венера

        Bitmap img;
        public Filters(Bitmap img)
        {
            this.img = img;
        }
        public Bitmap Sepia()
        {
            Color p;
            Bitmap _img = img; // создаём Bitmap из изображения, находящегося в pictureBox1

            for (int j = 0; j < _img.Height; j++)
                for (int i = 0; i < _img.Width; i++) // перебираем в циклах все пиксели исходного изображения
                {
                    p = _img.GetPixel(i, j);
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);
                    if (tr > 255)
                        r = 255;
                    else
                        r = tr;
                    if (tg > 255)
                        g = 255;
                    else
                        g = tg;
                    if (tb > 255)
                        b = 255;
                    else
                        b = tb;
                    _img.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                }
            return _img;
        }
        public Bitmap BlackAndWhite() {

            Color p;
            Bitmap _img = img; // создаём Bitmap из изображения, находящегося в pictureBox1          
            for (int j = 0; j < _img.Height; j++)
                for (int i = 0; i < _img.Width; i++) // перебираем в циклах все пиксели исходного изображения
                {
                    p = _img.GetPixel(i, j);
                    int av = ((p.R + p.G + p.B) / 3);
                    if (av > 128)
                        _img.SetPixel(i, j, Color.White);
                    else
                        _img.SetPixel(i, j, Color.Black);
                }
            return _img;
        }
        public Bitmap Blur()
        {
            {

                Bitmap _img = img;
                _img = Blur(_img, 5);
                return _img;
            }
        }
        private static Bitmap Blur(Bitmap image, Int32 blurSize)
        {
            return Blur(image, new Rectangle(0, 0, image.Width, image.Height), blurSize);
        }
        private static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // сделайте точную копию предоставленного растрового изображения
            using (Graphics graphics = Graphics.FromImage(blurred))

                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // // посмотрите на каждый пиксель в прямоугольнике размытия
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    // усредните цвет красного, зеленого и синего для каждого пикселя в
                    // размере размытия, убедившись, что вы не выходите за границы изображения
                    for (int x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // теперь, когда мы знаем среднее значение размера размытия, установите для каждого пикселя этот цвет
                    for (int x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (int y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }
            return blurred;
        }
    
        public Bitmap ShadesOfGrey() {
                Color p;
                Bitmap _img = img; // создаём Bitmap из изображения, находящегося в pictureBox1

                for (int i = 0; i < _img.Width; i++)
                    for (int j = 0; j < _img.Height; j++)
                    {
                        //получить пиксель из исходного изображения
                        Color originalColor = _img.GetPixel(i, j);

                        //создать версию пикселя
                        int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                          + (originalColor.B * .11));

                        //создать цветной объект
                        Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                        // установить пиксель нового изображения
                        _img.SetPixel(i, j, newColor);
                    }
                return _img;
            }
    
        public Bitmap Sharpness() {
                    Bitmap _img = img;

                    int filterWidth = 3;
                    int filterHeight = 3;
                    int w = _img.Width;
                    int h = _img.Height;

                    double[,] filter = new double[filterWidth, filterHeight];

                    filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
                    filter[1, 1] = 9;

                    double factor = 1.0;
                    double bias = 0.0;

                    Color[,] result = new Color[w, h];

                    for (int x = 0; x < w; ++x)
                        for (int y = 0; y < h; ++y)
                        {
                            double red = 0.0, green = 0.0, blue = 0.0;


                            // Color must be read per filter entry, not per image pixel.
                            Color imageColor = _img.GetPixel(x, y);


                            for (int filterX = 0; filterX < filterWidth; filterX++)
                            {
                                for (int filterY = 0; filterY < filterHeight; filterY++)
                                {
                                    int imageX = (x - filterWidth / 2 + filterX + w) % w;
                                    int imageY = (y - filterHeight / 2 + filterY + h) % h;

                                    // Get the color here - once per fiter entry and image pixel.
                                    Color imageColor2 = _img.GetPixel(imageX, imageY);

                                    red += imageColor2.R * filter[filterX, filterY];
                                    green += imageColor2.G * filter[filterX, filterY];
                                    blue += imageColor2.B * filter[filterX, filterY];
                                }
                                int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                                int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                                int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                                result[x, y] = Color.FromArgb(r, g, b);
                            }
                        }
                    
                    for (int i = 0; i < w; ++i)
                        for (int j = 0; j < h; ++j)
                        {
                            _img.SetPixel(i, j, result[i, j]);
                        }
            return _img;
        }
        public Bitmap Negative()
        {
            Bitmap _img = img; // создаём Bitmap из изображения, находящегося в pictureBox1

            for (int j = 0; j < _img.Width; j++)// перебираем в циклах все пиксели исходного изображения
                for (int i = 0; i < _img.Height; i++)
                {
                    Color pixelColor = _img.GetPixel(j, i);
                    Color newColor = Color.FromArgb(0xff - pixelColor.R
                    , 0xff - pixelColor.G, 0xff - pixelColor.B);
                    _img.SetPixel(j, i, newColor);
                }
            return _img;
        }
                
        public Bitmap Sobel() {
            int x = 0, y = 0;
            int valxR = 0, valyR = 0, valxG = 0, valyG = 0, gradiant1, gradiant2, gradiant3;
            Bitmap _img = new Bitmap(img.Width, img.Height);
            Color c = Color.FromArgb(0, 0, 0);

            for (int i = 0; i < _img.Height; i++)
                for (int j = 0; j < _img.Width; j++)
                {

                    //for x axis
                    int[,] gx = new int[3, 3];
                    int[,] gy = new int[3, 3];

                    gx[0, 0] = -1; gx[0, 1] = 0; gx[0, 2] = 1;
                    gx[1, 0] = -2; gx[1, 1] = 0; gx[1, 2] = 2;
                    gx[2, 0] = -1; gx[2, 1] = 0; gx[2, 2] = 1;

                    //////y
                    gy[0, 0] = -1; gy[0, 1] = -2; gy[0, 2] = -1;
                    gy[1, 0] = 0; gy[1, 1] = 0; gy[1, 2] = 0;
                    gy[2, 0] = 1; gy[2, 1] = 2; gy[2, 2] = 1;



                    if ((i == 0) || i == (_img.Height - 1) || (j == 0) || j == (img.Width - 1))
                    {
                        c = Color.FromArgb(255, 255, 255);
                        _img.SetPixel(j, i, c);
                        valxR = valyR = valxG = valyG = 0;

                    }
                    else
                    {
                        valxR = (img.GetPixel(j - 1, i - 1).R * gx[0, 0])
                            + (img.GetPixel(j + 1, i - 1).R * gx[0, 2])
                            + (img.GetPixel(j - 1, i).R * gx[1, 0])
                            + (img.GetPixel(j + 1, i).R * gx[1, 2])
                            + (img.GetPixel(j - 1, i + 1).R * gx[2, 0])
                            + (img.GetPixel(j + 1, i + 1).R * gx[2, 2]);

                        valyR = (img.GetPixel(j - 1, i - 1).R * gy[0, 0])
                            + (img.GetPixel(j, i - 1).R * gy[0, 1])
                            + (img.GetPixel(j + 1, i - 1).R * gy[0, 2])
                            + (img.GetPixel(j - 1, i + 1).R * gy[2, 0])
                            + (img.GetPixel(j, i + 1).R * gy[2, 1])
                            + (img.GetPixel(j + 1, i + 1).R * gy[2, 2]);
                        gradiant1 = (int)Math.Sqrt((valxR * valxR) + (valyR * valyR));
                        gradiant2 = gradiant1;
                        if (valxR != 0)
                            gradiant2 = (int)Math.Abs(Math.Atan(valyR / valxR) * 180);


                        if (gradiant1 < 0)
                        {
                            gradiant1 = 0;
                   

                        }

                        if (gradiant1 > 255)
                        {
                            gradiant1 = 255;

                            c = Color.FromArgb(gradiant1, gradiant1, gradiant1);
                            if (gradiant2 <= 30)
                                c = Color.FromArgb(250, 80, 20);
                            else if (gradiant2 <= 60)
                                c = Color.FromArgb(250, 0, 0);
                            if (gradiant2 <= 60)
                                c = Color.FromArgb(250, 0, 0);
                            else if (gradiant2 <= 120)
                                c = Color.FromArgb(0, 250, 0);
                            else if (gradiant2 <= 180)
                                c = Color.FromArgb(0, 0, 250);
                            else if (gradiant2 <= 270)
                                c = Color.FromArgb(250, 250, 0);
                            else if (gradiant2 <= 270)
                                c = Color.FromArgb(0, 250, 250);
                            else if (gradiant2 <= 360)
                                c = Color.FromArgb(250, 0, 250);

                            _img.SetPixel(j, i, c);

                            continue;
                        }
                        c = Color.FromArgb(gradiant1, gradiant1, gradiant1);
                        _img.SetPixel(j, i, c);

                    }


                }
            return _img;
        }
        public Bitmap Roberts() {


            return img;


        }
        public Bitmap AdaptiveNoiseReduction() { return img; }
        public Bitmap WienerFrequency() {
            return img;
        }

    }
}
