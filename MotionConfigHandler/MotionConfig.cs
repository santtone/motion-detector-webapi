
namespace MotionConfigHandler
{
    public class MotionConfig
    {
        [MotionConfigProperty("width")]
        public int? CaptureWidth { get; set; }

        [MotionConfigProperty("height")]
        public int? CaptureHeight { get; set; }
    }
}
