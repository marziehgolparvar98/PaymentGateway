using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Text;


namespace PaymentGateway.Common
{
    public class CaptchaAttribute : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string errorMessage = "کد امنیتی اشتباه وارد شده است";
            var captcha = context.ActionArguments.Select(s => s.Value).OfType<ICaptcha>().FirstOrDefault();
            if (captcha?.Captcha == null)
            {
                context.Result = new BadRequestObjectResult(new { Message = errorMessage, Code = -1000 });
                return;
            }

            if (!CaptchaRepository.AllCaptcha.TryGetValue($"{captcha.Captcha.Hash}-{captcha.Captcha.Salt}", out var value))
            {
                context.Result = new BadRequestObjectResult(new { Message = errorMessage, Code = -1000 });
                return;

            }

            if (!CaptchaHelper.Validate(captcha.Captcha))
            {
                context.Result = new BadRequestObjectResult(new { Message = errorMessage, Code = -1000 });
                return;
            }
            CaptchaRepository.AllCaptcha.TryRemove($"{captcha.Captcha.Hash}-{captcha.Captcha.Salt}", out var _);
        }
    }

    public class CaptchaItem
    {
        public string Value { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;

        public string Hash { get; set; } = string.Empty;
    }
    public interface ICaptcha
    {
        CaptchaItem Captcha { get; set; }
    }
    public static class CaptchaRepository
    {
        public static ConcurrentDictionary<string, bool> AllCaptcha = new ConcurrentDictionary<string, bool>();
    }

    public class CaptchaResult
    {
        public byte[] CaptchaByteData { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string HashedCaptcha { get; set; } = null!;
    }

    public static class Captcha
    {
        private const string Letters = "1234567890";
        public static CaptchaResult GenerateCaptchaImage(ref string captchaCode, int width = 120, int height = 30)
        {
            if (string.IsNullOrEmpty(captchaCode))
                captchaCode = GenerateCaptchaCode();


            var salt
                = GetSalt();
            return new CaptchaResult
            {
                CaptchaByteData = GenerateImage(captchaCode),
                Salt = salt,
                HashedCaptcha = captchaCode.Sha256(salt)
            };

            byte[] GenerateImage(string _captchaCode)
            {
                int iHeight = 52;
                int iWidth = 124;
                Random oRandom = new Random();

                int[] aBackgroundNoiseColor = new int[] { 100, 100, 100 };
                int[] aTextColor = new int[] { 0, 0, 0 };
                int[] aFontEmSizes = new int[] { 20, 25, };

                string[] aFontNames = new string[]
                {

                    "Comic Sans MS",
                    "Arial",
                    "Times New Roman",
                    "Georgia",
                    "Verdana",
                    "Geneva"
                };
                FontStyle[] aFontStyles = new FontStyle[]
                {
                    FontStyle.Bold,
                    FontStyle.Italic,
                    FontStyle.Regular,
                    FontStyle.Underline
                };
                HatchStyle[] aHatchStyles = new HatchStyle[]
                {


                    HatchStyle.BackwardDiagonal, HatchStyle.Cross,
                    HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
                    HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
                    HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
                    HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                    HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
                    HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
                    HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
                    HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
                    HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
                    HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
                    HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
                    HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
                    HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
                    HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
                    HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
                };


                string sCaptchaText = _captchaCode;


                Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
                oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;


                RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
                Brush oBrush = default!;


                oBrush = new HatchBrush(aHatchStyles[oRandom.Next
                    (aHatchStyles.Length - 1)], Color.FromArgb(oRandom.Next(100, 255),
                    oRandom.Next(100, 255), oRandom.Next(100, 255)), Color.White);
                oGraphics.FillRectangle(oBrush, oRectangleF);

                Matrix oMatrix = new Matrix();
                int i = 0;
                for (i = 0; i <= sCaptchaText.Length - 1; i++)
                {
                    oMatrix.Reset();
                    int iChars = sCaptchaText.Length;
                    int x = iWidth / (iChars + 1) * i;
                    int y = iHeight;


                    oMatrix.RotateAt(oRandom.Next(-10, 15), new PointF(x, y));
                    oGraphics.Transform = oMatrix;


                    oGraphics.DrawString
                    (

                    sCaptchaText.Substring(i, 1),

                    new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                       aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                       aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),

                    new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                       oRandom.Next(0, 100), oRandom.Next(0, 100))),
                    x,
                    oRandom.Next(10, 20)
                    );
                    oGraphics.ResetTransform();
                }

                using (MemoryStream oMemoryStream = new MemoryStream())
                {
                    oOutputBitmap.Save(oMemoryStream, ImageFormat.Jpeg);
                    return oMemoryStream.ToArray();
                }

            }//end local methods
        }

        private static string GenerateCaptchaCode()
        {
            var rand = new Random();
            var maxRand = Letters.Length - 1;

            var sb = new StringBuilder();

            for (var i = 0; i < 5; i++)
            {
                var index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }
        private static string Sha256(this string text, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var @now = DateTime.Now;
                var timeString = string.Format("{0}-{1}-{2} {3}:{4}",
                                    @now.Year,
                                    @now.Month,
                                    @now.Day,
                                    now.Hour.ToString("00"),//hour
                                    (now.Minute - now.Minute % 10).ToString("00"));//minute


                var saltedValue = Encoding.UTF8
                                          .GetBytes(text)
                                          .Concat(Encoding.UTF8.GetBytes(salt + timeString))
                                          .ToArray();

                // Send a sample text to hash.  
                var firstHashedBytes = sha256.ComputeHash(saltedValue);
                var secondHashedBytes = sha256.ComputeHash(firstHashedBytes);
                var thirdHashedBytes = sha256.ComputeHash(secondHashedBytes);
                // Get the hashed string.  
                return $"{@now.Hour.ToString("00")}:{@now.Minute.ToString("00")}:{BitConverter.ToString(thirdHashedBytes)}";
            }
        }
        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes);
            }
        }

    }


}
