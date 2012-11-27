using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCriminal : XmlAttachment
    {
        private Nation m_CriminalNation;
        private Mobile m_Attacher;

        [CommandProperty(AccessLevel.GameMaster)]
        public Nation CriminalNation
        {
            get
            {
                return m_CriminalNation;
            }
            set
            {
                m_CriminalNation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Attacher
        {
            get { return m_Attacher; }
            set { m_Attacher = value; }
        }

        public XmlCriminal(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlCriminal()
        {
        }

        [Attachable]
        public XmlCriminal(Nation n) : this(new Soldier(n))
        {
        }

        [Attachable]
        public XmlCriminal(Mobile attacher)
        {
            m_Attacher = attacher;

            if ((Attacher is PlayerMobile) && (Attacher as PlayerMobile).Nation != null && (Attacher as PlayerMobile).Nation != Nation.None)
                m_CriminalNation = (Attacher as PlayerMobile).Nation;
            else if ((Attacher is BaseCreature) && (Attacher as BaseCreature).Nation != null && (Attacher as BaseCreature).Nation != Nation.None)
                m_CriminalNation = (Attacher as BaseCreature).Nation;
            else
                Delete();

            Name = m_CriminalNation.ToString();
        }

        public override string OnIdentify(Mobile from)
        {
            base.OnIdentify(from);

            if (from == null || from.AccessLevel == AccessLevel.Player) return null;

            if (Expiration > TimeSpan.Zero)
            {
                return String.Format("Criminal status expires in {1} minutes.", Expiration.TotalMinutes);
            }
            else
            {
                return String.Format("XMLCriminal Expiration <= 0");
            }
        }

        public override void OnAttach()
        {
            base.OnAttach();

            if ((Attacher is Soldier) && (Attacher as Soldier).Nation == Nation.None)
            {
                Delete();
                return;
            }

            if (AttachedTo is PlayerMobile)
            {
                if (!(AttachedTo as PlayerMobile).CrimesList.ContainsKey(CriminalNation))
                    Delete();

                ((PlayerMobile)AttachedTo).CrimesList[CriminalNation] += Utility.RandomMinMax(5, 10);
                this.Expiration = TimeSpan.FromMinutes(((PlayerMobile)AttachedTo).CrimesList[CriminalNation]);
            }
            else if (AttachedTo is BaseCreature)
            {
                if (((BaseCreature)AttachedTo).Controlled && Attacher.CanSee(((BaseCreature)AttachedTo).Controlled))
                {
                    PlayerMobile controlMaster = (PlayerMobile)((BaseCreature)AttachedTo).ControlMaster;

                    if (!controlMaster.CrimesList.ContainsKey(CriminalNation))
                        Delete();

                    controlMaster.CrimesList[CriminalNation] += Utility.RandomMinMax(5, 10);
                    this.Expiration = TimeSpan.FromMinutes(controlMaster.CrimesList[CriminalNation]);
                    XmlAttach.AttachTo(controlMaster, new XmlCriminal(Attacher));
                }

                this.Expiration = TimeSpan.FromMinutes(120.0);
            }
            else
                Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)m_CriminalNation);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            m_CriminalNation = (Nation) reader.ReadInt();
            int version = reader.ReadInt();

            Delete();
        }
    }
}
