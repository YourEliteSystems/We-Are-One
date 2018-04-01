using System;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;
using Un4seen.Bass.Misc;
using System.IO;

namespace We_Are_One
{
    internal class MusicPlayer
    {
        private string[] metaTags;
        private int stream = 0;
        private string path = "";
        private double volume;
        public TAG_INFO Tags;
        public Visuals Spectrum = new Visuals();
        private bool Playing;

        public void InitialBass()
        {
            Bass.BASS_Init(-1,44100,0,IntPtr.Zero);
        }

        public void Play(string Filename)
        {
            Bass.BASS_StreamFree(this.stream);
            this.path = Filename;
            this.volume *= 0.01;
            this.stream = Bass.BASS_StreamCreateFile(this.path, 0L, 0L, 0);
            if (this.stream != 0)
            {
                Bass.BASS_ChannelPlay(this.stream, false);
                this.Tags = new TAG_INFO(this.path);
                BassTags.BASS_TAG_GetFromFile(this.stream, this.Tags);
            }
        }

        public void SetLautstaerke(double lautstaerkeInProzent)
        {
            volume = lautstaerkeInProzent * 0.01;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)volume);
        }

        public int GetCurrentPosition()
        {
            return (int)Bass.BASS_ChannelBytes2Seconds(this.stream, Bass.BASS_ChannelGetPosition(this.stream));
        }

        public int GetDuration()
        {
            return (int)Bass.BASS_ChannelBytes2Seconds(this.stream, Bass.BASS_ChannelGetLength(this.stream));
        }

        public void SetCurentposition(int CurrentPosition)
        {
            Bass.BASS_ChannelSetPosition(this.stream, (double)CurrentPosition);
        }

        public void Stop()
        {
            Bass.BASS_ChannelStop(stream);
        }

        public bool IsPlaying()
        {
            if (Bass.BASS_ChannelIsActive(stream) == 0)
            {
                this.Playing = false;
            }
            else
            {
                this.Playing = true;
            }
            return this.Playing;
        }

        public void Pause()
        {
            if (Bass.BASS_ChannelIsActive(stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Bass.BASS_ChannelPause(stream);
            }
            else if (Bass.BASS_ChannelIsActive(stream) != BASSActive.BASS_ACTIVE_PLAYING)
            {
                Bass.BASS_ChannelPlay(stream, false);
            }
        }

        public int GetVolume()
        {
            return (int)(this.volume * 100.0);
        }

        public bool PlayStream(string url, double lautstaerkeInProzent)
        {
            volume = (double)lautstaerkeInProzent * 0.01;
            if (IsPlaying())
            {
                Stop();
            }
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 0);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 300);
            this.stream = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_AUTOFREE, null, IntPtr.Zero);
            bool result;
            if (stream != 0)
            {
                TAG_INFO tAG_INFO = new TAG_INFO(url);
                if (BassTags.BASS_TAG_GetFromURL(stream, tAG_INFO))
                {
                    string text = string.Concat(new string[]
                    {
                        tAG_INFO.artist.ToString(),
                        ";",
                        tAG_INFO.title.ToString(),
                        ";",
                        tAG_INFO.genre.ToString()
                    });
                    metaTags = text.Split(new char[]
                    {
                        ';'
                    });
                }
                Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, (float)volume);
                Bass.BASS_ChannelPlay(stream, false);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        /*
        public void Analyzer(PictureBox analyzer, Color anColor)
        {
            if (analyzer != null)
            {
                analyzer.Image = this.Spectrum.CreateSpectrumLine(this.stream, analyzer.Width, analyzer.Height, Color.White, anColor, Color.Transparent, 5, 10, false, true, true);
            }
        }*/

        public string[] GetMetaTags()
        {
            return this.metaTags;
        }

        public bool GetUrlCheck(string url)
        {
            bool result;
            if (Path.GetExtension(url) == "m3u")
            {
                result = false;
            }
            else
            {
                int num = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_AUTOFREE, null, IntPtr.Zero);
                result = (num != 0);
            }
            return result;
        }
    }
}
