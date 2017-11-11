using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs;
using CSCore.DSP;
using CSCore.SoundOut;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Streams.Effects;
using WeifenLuo.WinFormsUI.Docking;
using EditorMC2D.Events;
using EditorMC2D.Docking.Docment.Audio;
using EditorMC2D.Common;

namespace EditorMC2D.Docking.Docment
{
    public partial class AudioDoc : DockContent
    {
        public DocumentFocusHandler DocumentFocusEvent;

        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private PitchShifter _pitchShifter;
        private LineSpectrum _lineSpectrum;
        private VoicePrint3DSpectrum _voicePrint3DSpectrum;

        private readonly Bitmap _bitmap = new Bitmap(2000, 600);
        private int _xpos;
        private bool _stopSliderUpdate=false;
        private string m_fileFullPath;
        private string m_filePath;
        /// <summary>
        /// scディレクトリ以降のファイルパス
        /// </summary>
        public string FilePath { get { return m_filePath; } }
        /// <summary>
        /// ファイルのフルパス
        /// </summary>
        public string FileFullPath { get { return m_fileFullPath; } }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public AudioDoc(string filePath)
        {
            InitializeComponent();
            m_filePath = filePath;
            m_fileFullPath = CommonMC2D.Instance.DirPathMC2D + @"\" + filePath;
            this.Icon = Icon.FromHandle(Properties.Resources.SoundFile_16x.GetHicon());

            Open();
        }
        /// <summary>
        /// 再生位置
        /// </summary>
        public TimeSpan Position
        {
            get
            {
                if (_source != null)
                    return _source.GetPosition();
                return TimeSpan.Zero;
            }
            set
            {
                if (_source != null)
                    _source.SetPosition(value);
            }
        }
        /// <summary>
        /// サウンドの長さ
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                if (_source != null)
                    return _source.GetLength();
                return TimeSpan.Zero;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DocumentFocus(Object sender, EventArgs e)
        {
            if (DocumentFocusEvent != null)
                DocumentFocusEvent(this, new DocumentFocusEventArgs(this));
        }
        /// <summary>
        /// サウンドファイルを開く
        /// </summary>
        private void Open()
        {
            Stop();

            //open the selected file
            ISampleSource source = CodecFactory.Instance.GetCodec(m_fileFullPath)
                .ToSampleSource()
                .AppendSource(x => new PitchShifter(x), out _pitchShifter);

            SetupSampleSource(source);

            _soundOut = new WasapiOut();
            _soundOut.Initialize(_source);


            propertyGridTop.SelectedObject = _lineSpectrum;
            propertyGridBottom.SelectedObject = _voicePrint3DSpectrum;

            endTimeLabel.Text = Length.ToString(@"mm\:ss\.FFF");
        }
        /// <summary>
        /// 再生する
        /// </summary>
        private void Play()
        {
            _soundOut.Play();
            timer1.Start();
        }
        /// <summary>
        /// ポーズする
        /// </summary>
        private void Pause()
        {
            _soundOut.Pause();
            timer1.Stop();
        }
        /// <summary>
        /// デフォルトのサウンドデバイスにセットする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fromDefaultDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();

            //open the default device 
            _soundIn = new WasapiLoopbackCapture();
            //Our loopback capture opens the default render device by default so the following is not needed
            //_soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            _soundIn.Initialize();

            var soundInSource = new SoundInSource(_soundIn);
            ISampleSource source = soundInSource.ToSampleSource().AppendSource(x => new PitchShifter(x), out _pitchShifter);

            SetupSampleSource(source);

            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                int read;
                while ((read = _source.Read(buffer, 0, buffer.Length)) > 0) ;
            };


            //play the audio
            _soundIn.Start();

            timer1.Start();

            propertyGridTop.SelectedObject = _lineSpectrum;
            propertyGridBottom.SelectedObject = _voicePrint3DSpectrum;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSampleSource"></param>
        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _lineSpectrum = new LineSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                BarCount = 50,
                BarSpacing = 2,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };
            _voicePrint3DSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = true,
                PointCount = 200,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Sqrt
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);

        }
        /// <summary>
        /// このドキュメントが閉じられた
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            timer1.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_soundIn != null)
            {
                _soundIn.Stop();
                _soundIn.Dispose();
                _soundIn = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            timer1.Stop();
            Position = TimeSpan.FromMilliseconds(0);
            currentTimeLabel.Text = Position.ToString(@"mm\:ss\.FFF");
            trackBar1.Value = 0;
            startPauseBtn.Image = Properties.Resources.Run_16x;
            stopBtn.Enabled = false;

            if (_soundOut != null)
            {
                _soundOut.Stop();
            }
            //if (_soundIn != null)
            //{
            //    _soundIn.Stop();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //render the spectrum
            GenerateLineSpectrum();
            GenerateVoice3DPrintSpectrum();


            TimeSpan length = Length;
            if (Position > length)
                length = Position;


            currentTimeLabel.Text = Position.ToString(@"mm\:ss\.FFF");

            if (length == Position)
            {
                Stop();
            }
            else if (stopBtn.Enabled &&
                length != TimeSpan.Zero && Position != TimeSpan.Zero)
            {
                double perc = Position.TotalMilliseconds / length.TotalMilliseconds * trackBar1.Maximum;
                trackBar1.Value = (int)perc;
            }
        }

        private void GenerateLineSpectrum()
        {
            Image image = pictureBoxTop.Image;
            var newImage = _lineSpectrum.CreateSpectrumLine(pictureBoxTop.Size, Color.Green, Color.Red, Color.Black, true);
            if (newImage != null)
            {
                pictureBoxTop.Image = newImage;
                if (image != null)
                    image.Dispose();
            }
        }

        private void GenerateVoice3DPrintSpectrum()
        {
            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                pictureBoxBottom.Image = null;
                if (_voicePrint3DSpectrum.CreateVoicePrint3D(g, new RectangleF(0, 0, _bitmap.Width, _bitmap.Height),
                    _xpos, Color.Black, 3))
                {
                    _xpos += 3;
                    if (_xpos >= _bitmap.Width)
                        _xpos = 0;
                }
                pictureBoxBottom.Image = _bitmap;
            }
        }
        /// <summary>
        /// ピッチシフター
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pitchShiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Form()
            {
                Width = 250,
                Height = 70,
                Text = String.Empty
            };
            TrackBar trackBar = new TrackBar()
            {
                TickStyle = TickStyle.None,
                Minimum = -100,
                Maximum = 100,
                Value = (int)(_pitchShifter != null ? Math.Log10(_pitchShifter.PitchShiftFactor) / Math.Log10(2) * 120 : 0),
                Dock = DockStyle.Fill
            };
            trackBar.ValueChanged += (s, args) =>
            {
                if (_pitchShifter != null)
                {
                    _pitchShifter.PitchShiftFactor = (float)Math.Pow(2, trackBar.Value / 120.0);
                    form.Text = trackBar.Value.ToString();
                }
            };
            form.Controls.Add(trackBar);

            form.ShowDialog();

            form.Dispose();
        }
        /// <summary>
        /// ストップボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopBtn_Click(object sender, EventArgs e)
        {
            stopBtn.Enabled = false;
            Stop();
        }

        private void splitContainer2_Panel2_Resize(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 再生・一時停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playPauseBtn_Click(object sender, EventArgs e)
        {
            stopBtn.Enabled = true;
            if (_soundOut.PlaybackState == PlaybackState.Paused || _soundOut.PlaybackState == PlaybackState.Stopped)
            {
                Play();
                startPauseBtn.Image = Properties.Resources.Pause_16x;
            }
            else if (_soundOut.PlaybackState == PlaybackState.Playing)
            {
                Pause();
                startPauseBtn.Image = Properties.Resources.Run_16x;
            }
        }

        #region トラッカー
        /// <summary>
        /// 再生位置の移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_stopSliderUpdate)
            {
                double perc = trackBar1.Value / (double)trackBar1.Maximum;
                TimeSpan position = TimeSpan.FromMilliseconds(Length.TotalMilliseconds * perc);
                Position = position;
                currentTimeLabel.Text = Position.ToString(@"mm\:ss\.FFF");
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _stopSliderUpdate = false;
        }
        #endregion
    }
}
