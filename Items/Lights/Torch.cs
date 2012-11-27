using System;
using Server;

namespace Server.Items
{
	public class Torch : BaseEquipableLight
	{
		public override int LitItemID{ get { return 0xA12; } }
		public override int UnlitItemID{ get { return 0xF6B; } }

		public override int LitSound{ get { return 1209; } }
		public override int UnlitSound{ get { return 1208; } }
		
		[Constructable]
		public Torch() : base( 0xF6B )
		{
			if ( Burnout )
				Duration = TimeSpan.FromMinutes( 30 );
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Circle300;
			Weight = 1.0;
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );
		}

		public override void Ignite()
		{
			base.Ignite();
			Timer.DelayCall( TimeSpan.FromSeconds( 0.25 ), new TimerStateCallback( IgniteFireCallback ), this );
			BurnCallback( this );
		}
		
		public static void IgniteFireCallback( object state )
		{
			Torch torch = state as Torch;
			if ( torch.Parent is Mobile && torch.Burning && !torch.Deleted ) 
				((Mobile)torch.Parent).PlaySound( 84 );
		}
		
		public static void BurnCallback( object state )
		{
			Torch torch = state as Torch;
			if ( torch.Deleted || !( torch.Parent is Mobile ) || !torch.Burning )
				return;
			else
			{
				((Mobile)torch.Parent).PlaySound( 1210 );
				Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( BurnCallback ), state );
			}
		}

		public Torch( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}
}
