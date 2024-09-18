using SkiaSharp;
using System.Text;

namespace BootstrapAdmin.Library.Utilities {
    public class VerifyCode {
        private readonly Random _random = new();

        #region -- 設置 --

        /// <summary>
        /// 驗證碼長度
        /// </summary>
        public int CodeLength { get; set; } = 4;
        /// <summary>
        /// 驗證碼字符串
        /// </summary>
        public string VerifyCodeText { get; set; } = null!;
        /// <summary>
        /// 是否加入小寫字母
        /// </summary>
        public bool HasLowerLetter { get; set; } = true;
        /// <summary>
        /// 是否加入大寫字母
        /// </summary>
        public bool HasUpperLetter { get; set; } = true;
        /// <summary>
        /// 字體大小
        /// </summary>
        public int CodeFontSize { get; set; } = 36;
        /// <summary>
        /// 字體顏色
        /// </summary>
        public SKColor CodeFontColor { get; set; } = SKColors.Blue;

        /// <summary>
        /// 字體類型
        /// </summary>
        public string CodeFontFamily = "Verdana";
        /// <summary>
        /// 背景顏色
        /// </summary>
        public SKColor CodeBackgroundColor { get; set; } = SKColors.AliceBlue;
        /// <summary>
        /// 是否加入背景線
        /// </summary>
        public bool HasBackgroundLine { get; set; } = true;

        /// <summary>
        /// 前景噪點數量
        /// </summary>
        public int ForeNoisePointCount { get; set; } = 2;
        /// <summary>
        /// 隨機碼的旋轉角度
        /// </summary>
        public int RandomCodeRotateAngle { get; set; } = 40;

        /// <summary>
        /// 是否隨機字體顏色
        /// </summary>
        public bool IsRandomColor { get; set; } = true;
        /// <summary>
        /// 圖片寬度
        /// </summary>
        public int ImageWidth { get; set; } = 200;
        /// <summary>
        /// 圖片高度
        /// </summary>
        public int ImageHeight { get; set; } = 40;

        /// <summary>
        /// 問題驗證碼答案，使用在運算式
        /// </summary>
        public string VerifyCodeResult { get; private set; } = null!;

        #endregion

        #region -- 建構子 --

        public VerifyCode(int length = 4, bool isOperation = false) {
            if (isOperation) {
                var dic = GetQuestion();
                VerifyCodeText = dic.Key;
                VerifyCodeResult = dic.Value;
                RandomCodeRotateAngle = 0;
            }
            else {
                CodeLength = length;
                GetVerifyCodeText();
            }
            ImageWidth = VerifyCodeText.Length * CodeFontSize;
            ImageHeight = Convert.ToInt32(60.0 / 100 * CodeFontSize + CodeFontSize);

            InitColors();
        }

        #endregion

        #region -- 私有方法 --

        /// <summary>
        /// 取得驗證碼字符串
        /// </summary>
        private void GetVerifyCodeText() {
            // 沒有外部輸入驗證碼時，隨機產生
            if (string.IsNullOrEmpty(VerifyCodeText)) {
                StringBuilder charSB = new();

                // 加入數字 1 ~ 9
                for (int i = 1; i <= 9; i++) {
                    charSB.Append(i);
                }

                // 加入大寫字母 A ~ Z，但不包括 O
                if (HasUpperLetter) {
                    for (int i = 0; i < 26; i++) {
                        char temp = Convert.ToChar(i + 65);

                        // 如果生成的字母不是 'O'
                        if (!temp.Equals('O')) {
                            charSB.Append(temp);
                        }
                    }
                }

                // 加入小寫字母 A ~ Z，但不包括 o
                if (HasLowerLetter) {
                    for (int i = 0; i < 26; i++) {
                        char temp = Convert.ToChar(i + 97);

                        // 如果生成的字母不是 'o'
                        if (!temp.Equals('o')) {
                            charSB.Append(temp);
                        }
                    }
                }

                // 產生驗證碼字符串
                for (int i = 0; i < CodeLength; i++) {
                    int rndNum = _random.Next(0, charSB.Length);
                    VerifyCodeText += charSB[rndNum];
                    charSB.Remove(rndNum, 1);
                }
            }
        }

        /// <summary>
        /// 取得隨機顏色
        /// </summary>
        /// <returns></returns>
        private static SKColor GetRandomColor() {
            Random rndNum_First = new((int) DateTime.Now.Ticks);
            Thread.Sleep(rndNum_First.Next(50));
            Random rndNum_Second = new((int) DateTime.Now.Ticks);
            // 為了在白色背景上顯示，盡量產生深色
            int int_Red = rndNum_First.Next(256);
            int int_Green = rndNum_Second.Next(256);
            int int_Blue = int_Red + int_Green > 400 ? 0 : 400 - int_Red - int_Green;
            int_Blue = int_Blue > 255 ? 255 : int_Blue;
            return SKColor.FromHsv(int_Red, int_Green, int_Blue);
        }

        #endregion

        #region -- 公有方法 --

        /// <summary>
        /// 取得題目、問題
        /// </summary>
        /// <param name="questionList">預設數字加減驗證</param>
        /// <returns></returns>
        public KeyValuePair<string, string> GetQuestion(Dictionary<string, string>? questionList = null) {
            if (questionList is null) {
                questionList = new();
                var operateArray = new string[] { "+", "*", "-", "/" };
                var left = _random.Next(0, 10);
                var right = _random.Next(0, 10);
                var operate = operateArray[_random.Next(0, operateArray.Length)];

                string key;
                string val;

                switch (operate) {
                    case "+":
                        key = string.Format("{0}+{1}=?", left, right);
                        val = (left + right).ToString();
                        questionList.Add(key, val);
                        break;
                    case "*":
                        key = string.Format("{0}×{1}=?", left, right);
                        val = (left * right).ToString();
                        questionList.Add(key, val);
                        break;
                    case "-":
                        if (left < right) {
                            (right, left) = (left, right);
                        }
                        key = string.Format("{0}-{1}=?", left, right);
                        val = (left - right).ToString();
                        questionList.Add(key, val);
                        break;
                    case "/":
                        right = _random.Next(1, 10);
                        left = right * _random.Next(1, 10);
                        key = string.Format("{0}÷{1}=?", left, right);
                        val = (left / right).ToString();
                        questionList.Add(key, val);
                        break;
                }
            }
            return questionList.ToList()[_random.Next(0, questionList.Count)];
        }

        #endregion

        #region -- 新編碼 --

        /// <summary>
        /// 干擾線的顏色列表
        /// </summary>
        private List<SKColor> Colors { get; set; } = new List<SKColor>();
        public void InitColors() {
            Colors = new List<SKColor> {
                SKColors.AliceBlue,
                SKColors.PaleGreen,
                SKColors.PaleGoldenrod,
                SKColors.Orchid,
                SKColors.OrangeRed,
                SKColors.Orange,
                SKColors.OliveDrab,
                SKColors.Olive,
                SKColors.OldLace,
                SKColors.Navy,
                SKColors.NavajoWhite,
                SKColors.Moccasin,
                SKColors.MistyRose,
                SKColors.MintCream,
                SKColors.MidnightBlue,
                SKColors.MediumVioletRed,
                SKColors.MediumTurquoise,
                SKColors.MediumSpringGreen,
                SKColors.LightSlateGray,
                SKColors.LightSteelBlue,
                SKColors.LightYellow,
                SKColors.Lime,
                SKColors.LimeGreen,
                SKColors.Linen,
                SKColors.PaleTurquoise,
                SKColors.Magenta,
                SKColors.MediumAquamarine,
                SKColors.MediumBlue,
                SKColors.MediumOrchid,
                SKColors.MediumPurple,
                SKColors.MediumSeaGreen,
                SKColors.MediumSlateBlue,
                SKColors.Maroon,
                SKColors.PaleVioletRed,
                SKColors.PapayaWhip,
                SKColors.PeachPuff,
                SKColors.Snow,
                SKColors.SpringGreen,
                SKColors.SteelBlue,
                SKColors.Tan,
                SKColors.Teal,
                SKColors.Thistle,
                SKColors.SlateGray,
                SKColors.Tomato,
                SKColors.Violet,
                SKColors.Wheat,
                SKColors.White,
                SKColors.WhiteSmoke,
                SKColors.Yellow,
                SKColors.YellowGreen,
                SKColors.Turquoise,
                SKColors.LightSkyBlue,
                SKColors.SlateBlue,
                SKColors.Silver,
                SKColors.Peru,
                SKColors.Pink,
                SKColors.Plum,
                SKColors.PowderBlue,
                SKColors.Purple,
                SKColors.Red,
                SKColors.SkyBlue,
                SKColors.RosyBrown,
                SKColors.SaddleBrown,
                SKColors.Salmon,
                SKColors.SandyBrown,
                SKColors.SeaGreen,
                SKColors.SeaShell,
                SKColors.Sienna,
                SKColors.RoyalBlue,
                SKColors.LightSeaGreen,
                SKColors.LightSalmon,
                SKColors.LightPink,
                SKColors.Crimson,
                SKColors.Cyan,
                SKColors.DarkBlue,
                SKColors.DarkCyan,
                SKColors.DarkGoldenrod,
                SKColors.DarkGray,
                SKColors.Cornsilk,
                SKColors.DarkGreen,
                SKColors.DarkMagenta,
                SKColors.DarkOliveGreen,
                SKColors.DarkOrange,
                SKColors.DarkOrchid,
                SKColors.DarkRed,
                SKColors.DarkSalmon,
                SKColors.DarkKhaki,
                SKColors.DarkSeaGreen,
                SKColors.CornflowerBlue,
                SKColors.Chocolate,
                SKColors.AntiqueWhite,
                SKColors.Aqua,
                SKColors.Aquamarine,
                SKColors.Azure,
                SKColors.Beige,
                SKColors.Bisque,
                SKColors.Coral,
                SKColors.Black,
                SKColors.Blue,
                SKColors.BlueViolet,
                SKColors.Brown,
                SKColors.BurlyWood,
                SKColors.CadetBlue,
                SKColors.Chartreuse,
                SKColors.BlanchedAlmond,
                SKColors.Transparent,
                SKColors.DarkSlateBlue,
                SKColors.DarkTurquoise,
                SKColors.IndianRed,
                SKColors.Indigo,
                SKColors.Ivory,
                SKColors.Khaki,
                SKColors.Lavender,
                SKColors.LavenderBlush,
                SKColors.HotPink,
                SKColors.LawnGreen,
                SKColors.LightBlue,
                SKColors.LightCoral,
                SKColors.LightCyan,
                SKColors.LightGoldenrodYellow,
                SKColors.LightGray,
                SKColors.LightGreen,
                SKColors.LemonChiffon,
                SKColors.DarkSlateGray,
                SKColors.Honeydew,
                SKColors.Green,
                SKColors.DarkViolet,
                SKColors.DeepPink,
                SKColors.DeepSkyBlue,
                SKColors.DimGray,
                SKColors.DodgerBlue,
                SKColors.Firebrick,
                SKColors.GreenYellow,
                SKColors.FloralWhite,
                SKColors.Fuchsia,
                SKColors.Gainsboro,
                SKColors.GhostWhite,
                SKColors.Gold,
                SKColors.Goldenrod,
                SKColors.Gray,
                SKColors.ForestGreen
            };
        }

        /// <summary>
        /// 創建畫筆
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        private static SKPaint CreatePaint(SKColor color, float fontSize) {
            var font = SKTypeface.FromFamilyName(
                null, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
            SKPaint paint = new() {
                IsAntialias = true,
                Color = color,
                Typeface = font,
                TextSize = fontSize
            };
            return paint;
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="lineNum">干擾線數量</param>
        /// <param name="lineStrookeWidth">干擾線寬度</param>
        /// <returns></returns>
        public byte[] GetVerifyCodeImage(int lineNum = 3, int lineStrookeWidth = 1) {
            // 創建Bitmap圖
            using SKBitmap image2d = new(ImageWidth, ImageHeight, SKColorType.Bgra8888, SKAlphaType.Premul);
            // 創建畫筆
            using SKCanvas canvas = new(image2d);

            if (IsRandomColor) {
                CodeFontColor = GetRandomColor();
            }

            // 填充白色背景
            canvas.Clear(CodeBackgroundColor);

            AddForeNoisePoint(image2d);
            AddBackgroundNoisePoint(image2d, canvas);

            // 將文字寫到 Canvas 上
            SKPaint drawStyle = new() {
                IsAntialias = true,
                TextSize = CodeFontSize
            };
            char[] chars = VerifyCodeText.ToCharArray();
            for (int i = 0; i < chars.Length; i++) {
                var font = SKTypeface.FromFamilyName(
                    CodeFontFamily, SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);

                // 轉動的角度
                float angle = _random.Next(-30, 30);

                canvas.Translate(12, 12);

                float px = i * CodeFontSize;
                float py = ImageHeight / 2;

                canvas.RotateDegrees(angle, px, py);

                drawStyle.Typeface = font;
                drawStyle.Color = CodeFontColor;
                // 寫字 (i + 1) * 16, 28
                canvas.DrawText(chars[i].ToString(), px, py, drawStyle);

                canvas.RotateDegrees(-angle, px, py);
                canvas.Translate(-12, -12);
            }

            // 畫隨機干擾線
            using (SKPaint disturbStyle = new()) {
                Random random = new();
                for (int i = 0; i < lineNum; i++) {
                    disturbStyle.Color = Colors[random.Next(Colors.Count)];
                    disturbStyle.StrokeWidth = lineStrookeWidth;
                    canvas.DrawLine(random.Next(0, ImageWidth), random.Next(0, ImageHeight), random.Next(0, ImageWidth), random.Next(0, ImageHeight), disturbStyle);
                }
            }

            // 返回圖片byte
            using var img = SKImage.FromBitmap(image2d);
            using SKData p = img.Encode(SKEncodedImageFormat.Png, 100);
            return p.ToArray();
        }

        private void AddForeNoisePoint(SKBitmap skBitmap) {
            for (int i = 0; i < skBitmap.Width * ForeNoisePointCount; i++) {
                skBitmap.SetPixel(_random.Next(skBitmap.Width), _random.Next(skBitmap.Height), CodeFontColor);
            }
        }

        private void AddBackgroundNoisePoint(SKBitmap skBitmap, SKCanvas skCanvas) {
            using (var pen = CreatePaint(SKColors.Azure, 0)) {
                for (int i = 0; i < skBitmap.Width * 2; i++) {
                    skCanvas.DrawRect(_random.Next(skBitmap.Width), _random.Next(skBitmap.Height), 1, 1, pen);
                }
            }

            if (HasBackgroundLine) {
                // 畫圖片的背景噪線
                for (int i = 0; i < 12; i++) {
                    var x1 = _random.Next(skBitmap.Width);
                    var x2 = _random.Next(skBitmap.Width);
                    var y1 = _random.Next(skBitmap.Height);
                    var y2 = _random.Next(skBitmap.Height);

                    skCanvas.DrawLine(x1, y1, x2, y2, CreatePaint(SKColors.Silver, 0));
                }
            }
        }

        #endregion
    }
}
