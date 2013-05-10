using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Rubble : Item
	{
		public override bool Decays { get { return true; } }

		[Constructable]
		public Rubble()
			: this(632, "")
		{ }

		[Constructable]
		public Rubble(int itemID, string namePrefix)
			: base(itemID)
		{
			Name = namePrefix + " Rubble";
			Movable = true;

			EnsureBeginDecay();
		}

		public Rubble(Serial serial)
			: base(serial)
		{ }

		private Timer _Decay;

		private void EnsureBeginDecay()
		{
			if (_Decay != null)
			{
				_Decay.Stop();
				_Decay = null;
			}

			_Decay = Timer.DelayCall(TimeSpan.FromMinutes(5.0), new TimerCallback(this.Delete));
		}

		public override bool OnMoveOver(Mobile m)
		{ return true; }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);//Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = (int)reader.ReadInt();

			EnsureBeginDecay();
		}
	}
}