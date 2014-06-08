using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Misc;
using Server.Network;

namespace Server
{
	public class SocketOptions
	{
		private const bool NagleEnabled = false; // Should the Nagle algorithm be enabled? This may reduce performance
		private const int CoalesceBufferSize = 512; // MSS that the core will use when buffering packets

		//private static int[] m_AdditionalPorts = new int[0];
		private static int[] m_AdditionalPorts = new int[]{ 2594 };

		public static void Initialize()
		{
			EventSink.SocketConnect += new SocketConnectEventHandler( EventSink_SocketConnect );
			SendQueue.CoalesceBufferSize = CoalesceBufferSize;

			if ( m_AdditionalPorts.Length > 0 )
				EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
		}

		public static void EventSink_ServerStarted()
		{
			for ( int i = 0; i < m_AdditionalPorts.Length; ++i )
				Core.MessagePump.AddListener( new Listener( m_AdditionalPorts[i] ) );
		}

		private static void EventSink_SocketConnect( SocketConnectEventArgs e )
		{
			if ( !e.AllowConnection )
				return;

			if ( !NagleEnabled )
				e.Socket.SetSocketOption( SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1 ); // RunUO uses its own algorithm
		}
                private static IPEndPoint[] m_ListenerEndPoints = new IPEndPoint[] {
                        new IPEndPoint( IPAddress.Any, 2594 ), // Shard Two will Listen on 2594
                     
                        // Examples:
                        // new IPEndPoint( IPAddress.Any, 80 ), // Listen on port 80 on all IP addresses
                        // new IPEndPoint( IPAddress.Parse( "1.2.3.4" ), 2593 ), // Listen on port 2593 on IP address 1.2.3.4
                };
 
	}
}
