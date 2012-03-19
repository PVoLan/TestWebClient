using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Collections.Generic;
using System.Timers;


/**************************************
 Calling GC.Collect() at the end causes an Exception
 
I/---------(26923): GC.Collect()
I/mono    (26923): Stacktrace:
I/mono    (26923): 
I/mono    (26923):   at System.Net.Sockets.Socket.Dispose (bool) <0x000b7>
I/mono    (26923):   at System.Net.Sockets.Socket.Finalize () <0x00027>
I/mono    (26923):   at (wrapper runtime-invoke) object.runtime_invoke_virtual_void__this__ (object,intptr,intptr,intptr) <0xffffffff>
I/---------(26923): GC.Collect() completed
 
 Also, sometimes GC.Collect() hangs application at all (unstable bug, sometimes hangs, sometimes not)
 
 Bug becomes apparent in RELEASE mode ONLY. In Debug mode all works properly.
 
 Bug looks similiar to another one I reported in November (support mail case #4365)

 */

namespace TestWebClient
{
	[Activity (Label = "TestWebClient", MainLauncher = true)]
	public class Activity1 : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			Android.Util.Log.Info("---------", "wc = new WebClient()");
			var wc = new WebClient();
			Android.Util.Log.Info("---------", "wc = new WebClient() completed");
			
			try
			{
				Android.Util.Log.Info("---------", "wc.DownloadData");
				var result = wc.DownloadData("http://google.com");
				Android.Util.Log.Info("---------", "wc.DownloadData completed");
			
			} 
			catch (Exception e)
			{
				Android.Util.Log.Info("---------", "Exception " + e.Message);
			} 
			finally 
			{
				Android.Util.Log.Info("---------", "wc.Dispose");
				wc.Dispose();	
				Android.Util.Log.Info("---------", "wc.Dispose completed");
			}
			
			Android.Util.Log.Info("---------", "GC.Collect()");
			GC.Collect(); //Exception here
			Android.Util.Log.Info("---------", "GC.Collect() completed");
		}
		
	}
}


