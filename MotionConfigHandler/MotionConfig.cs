namespace MotionConfigManager
{
    public class MotionConfig
    {
        public bool RecordMotion { get; set; }

        [MotionConfigProperty("width")]
        public string CaptureWidth { get; set; }

        [MotionConfigProperty("height")]
        public string CaptureHeight { get; set; }

        [MotionConfigProperty("framerate")]
        public string CaptureFrameRate { get; set; }

        [MotionConfigProperty("brightness")]
        public string CaptureBrightness { get; set; }

        [MotionConfigProperty("contrast")]
        public string CaptureContrast { get; set; }

        [MotionConfigProperty("quality")]
        public string ImageQuality { get; set; }
        
    }
}