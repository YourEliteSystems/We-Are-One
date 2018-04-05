using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace We_Are_One
{
	public class Bass_Net
	{
		
		#region Function Declerations
		private enum TagTypes : int {META=1,HTTP,ICY};
		private int Stream = 0;
		private readonly List<SYNCPROC> SyncProcs = new List<SYNCPROC>();
		private readonly List<int> SyncProcHandles = new List<int>();
		
		public delegate void METATagsRecieved(object sender,string[] e);
		public event METATagsRecieved OnMETATagsReceived;
		
		public delegate void HTTPTagsReceived(object sender, string[] e);
        public event HTTPTagsReceived OnHTTPTagsReceived;

        public delegate void ICYTagsReceived(object sender, string[] e);
        public event ICYTagsReceived OnICYTagsReceived;

        private float _volume = 0.5f;
		#endregion
		
		public float Volume{
			get{
				return _volume;
			}
			set{
				if(value < 0.0f|value>1.0f){
					throw new ArgumentOutOfRangeException("Volume must be in Range from 0 to 1");
				}
				_volume=value;
				if(Stream!=0){
					Bass.BASS_ChannelSetAttribute(Stream,BASSAttribute.BASS_ATTRIB_VOL,_volume);
				}
			}
		}
		
		public Bass_Net()
		{
			if(!Init()){
				MessageBox.Show(Bass.BASS_ErrorGetCode().ToString());
			}
		}
		
		private bool Init()
        {
            bool flag = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_CPSPEAKERS, IntPtr.Zero);
            flag &= Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 250); //250ms buffersize
            flag &= Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, true); //parse playlist  ((e.g. M3U or PLS or....)
            flag &= Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 250);  //250ms buffersize  
            flag &= Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 20); //buffer needs to be filled 20 before playing
            return flag;
        }
		
		private void MetaSyncProcCallback(int handle, int channel, int data, IntPtr user)
        {
            string[] tags;
            switch ((TagTypes)user.ToInt32())
            {
                case TagTypes.META:
                    tags = Bass.BASS_ChannelGetTagsMETA(channel);
                    if (tags != null) OnMETATagsReceived.Invoke(this, tags);
                    break;
                case TagTypes.HTTP:
                    tags = Bass.BASS_ChannelGetTagsHTTP(channel);
                    if (tags != null) OnHTTPTagsReceived.Invoke(this, tags);
                    break;
                case TagTypes.ICY:
                    tags = Bass.BASS_ChannelGetTagsICY(channel);
                    if (tags != null) OnICYTagsReceived.Invoke(this, tags);
                    break;
            }             
        }
		
		public bool PlayUrl(string url)
        {
            Stream = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE, null, IntPtr.Zero);
            bool flag = (Stream != 0);
            Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, _volume);           
            flag &= Bass.BASS_ChannelPlay(Stream, true);
            SyncProcs.Add(new SYNCPROC(MetaSyncProcCallback));
            SyncProcHandles.Add(Bass.BASS_ChannelSetSync(Stream, BASSSync.BASS_SYNC_META, 0, SyncProcs[SyncProcs.Count - 1], new IntPtr((int)TagTypes.HTTP)));
            SyncProcs.Add(new SYNCPROC(MetaSyncProcCallback));
            SyncProcHandles.Add(Bass.BASS_ChannelSetSync(Stream, BASSSync.BASS_SYNC_META, 0, SyncProcs[SyncProcs.Count - 1], new IntPtr((int)TagTypes.META)));
            SyncProcs.Add(new SYNCPROC(MetaSyncProcCallback));
            SyncProcHandles.Add(Bass.BASS_ChannelSetSync(Stream, BASSSync.BASS_SYNC_META, 0, SyncProcs[SyncProcs.Count - 1], new IntPtr((int)TagTypes.ICY)));
            return flag;
        }


        public bool PlayFile(string filename)
        {
            Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE);
            Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, _volume);
            return Bass.BASS_ChannelPlay(Stream, true);
        }


        public bool IsStreamActive()
        {
            return (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING) | (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STALLED) | (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PAUSED);
        }


        public void Stop(){
            if(Stream != 0)
            {
                while (SyncProcHandles.Count > 0)
                {
                    Bass.BASS_ChannelRemoveSync(Stream, SyncProcHandles[0]);
                    SyncProcHandles.RemoveAt(0);
                }
                while (SyncProcs.Count > 0)
                {
                    SyncProcs[0] = null;
                    SyncProcs.RemoveAt(0);
                }
                Bass.BASS_ChannelStop(Stream);
                Stream = 0;
            }
        }
	}
}
