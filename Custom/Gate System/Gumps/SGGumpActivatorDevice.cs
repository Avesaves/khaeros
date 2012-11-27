using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.SG
{
    public class SGGumpActivatorDevice : Gump
    {
        private SGActivatorDevice m_sgactivatordevice;

        public int sgTOselectioncounter;
        public int reset = 1;

        public int sgTOfacetid;
        public int sgTOaddress1;
        public int sgTOaddress2;
        public int sgTOaddress3;
        public int sgTOaddress4;
        public int sgTOaddress5;

        public int sgDESTINATIONfacet;
        public int sgDESTINATIONaddress1;
        public int sgDESTINATIONaddress2;
        public int sgDESTINATIONaddress3;
        public int sgDESTINATIONaddress4;
        public int sgDESTINATIONaddress5;

        public int sgDESTINATIONlocX;
        public int sgDESTINATIONlocY;
        public int sgDESTINATIONlocZ;
        public string sgDESTINATIONfacing;

        public bool sgDESTINATIONbusyused;
        public bool sgDESTINATIONbusyenergy;

        public int sgOnThisFacet = 0;
        public int sgOnThisHidden = 0;

        

        public SGGumpActivatorDevice(Mobile from, SGActivatorDevice m, int sgCURRENTsc, int sgCURRENTfacet, int sgCURRENTadd1, int sgCURRENTadd2, int sgCURRENTadd3, int sgCURRENTadd4, int sgCURRENTadd5) : base(100, 100)
        {
            m_sgactivatordevice = m;

            int SGSYB1 = 3676;
            int SGSYB2 = 3679;
            int SGSYB3 = 3682;
            int SGSYB4 = 3685;
            int SGSYB5 = 3688;

            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
			AddImage(350, 319, 2521);
			AddImage(190, 65, 2220);
			AddImage(219, 358, 2523);
			AddImage(257, 357, 2524);
			AddImage(521, 357, 2525);
			AddImage(354, 357, 2524);
			AddImage(519, 319, 2522);
			AddImage(246, 319, 2521);
			AddImage(219, 320, 2520);
			AddImage(219, 467, 2526);
			AddImage(257, 468, 2527);
			AddImage(522, 469, 2528);
			AddImage(353, 469, 2527);

            // THIS GATE ADDRESS
            //this.AddItem(255, 105, 
			//m.SGAFacetNumber * 3 - 3 + SGSYB1, 1360);
            this.AddItem(290, 105, 
			m.SGAAddressCode1 * 3 - 3 + SGSYB1, 2713);
			
            this.AddItem(255, 155, 
			m.SGAAddressCode2 * 3 - 3 + SGSYB1, 2713);
			
            this.AddItem(255, 205, 
			m.SGAAddressCode3 * 3 - 3 + SGSYB1, 2713);
			
            this.AddItem(315, 155,
			m.SGAAddressCode4 * 3 - 3 + SGSYB1, 2713);
			
            this.AddItem(315, 205, 
			m.SGAAddressCode5 * 3 - 3 + SGSYB1, 2713);
			
            // Dial Code Selection Icons
            // Facet option, starts with the facet the player is currently on
            //if (sgCURRENTfacet == 1)
           // {
              //  this.AddItem(260, 380, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);
                //this.AddLabel(305, 40, 35, @"Felucca");
            //}
            //else if (sgCURRENTfacet == 2)
           // {
            //    this.AddItem(260, 380, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);
                //this.AddLabel(305, 40, 70, @"Trammel");
           // }
            //else if (sgCURRENTfacet == 3)
           // {
            //    this.AddItem(260, 380, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);
                //this.AddLabel(305, 40, 55, @"Ilshenar");
           // }
          //  else if (sgCURRENTfacet == 4)
           // {
            //    this.AddItem(260, 380, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);
                //this.AddLabel(305, 40, 1265, @"Malas");
           // }
           // else if (sgCURRENTfacet == 5)
           // {
            //    this.AddItem(260, 380, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);
                //this.AddLabel(305, 40, 1152, @"Tokuno");
           // }

            this.AddItem(266, 360, SGSYB1, 2713);
            this.AddItem(316, 360, SGSYB2, 2713);
            this.AddItem(366, 360, SGSYB3, 2713); // this
            this.AddItem(416, 360, SGSYB4, 2713); // this
            this.AddItem(466, 360, SGSYB5, 2713);

            // Gate Selected By User
            //this.AddItem(417, 105, sgCURRENTfacet * 3 - 3 + SGSYB1, 1360);

            if (sgCURRENTsc == 1)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 1153);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 2946);
            }
            else if (sgCURRENTsc == 2)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 1153);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 2946);
            }
            else if (sgCURRENTsc == 3)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 1153);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 2946);
            }
            else if (sgCURRENTsc == 4)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 33);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 1153);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 2946);
            }
            else if (sgCURRENTsc == 5)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 33);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 33);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 1153);
            }
            else if (sgCURRENTsc >= 6)
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 33);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 33);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 33);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 33);
            }
            else
            {
                this.AddItem(452, 105, sgCURRENTadd1 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(417, 155, sgCURRENTadd2 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(417, 205, sgCURRENTadd3 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 155, sgCURRENTadd4 * 3 - 3 + SGSYB1, 2946);
                this.AddItem(477, 205, sgCURRENTadd5 * 3 - 3 + SGSYB1, 2946);
            }

            sgTOselectioncounter = sgCURRENTsc;
            sgTOfacetid = sgCURRENTfacet;
            sgTOaddress1 = sgCURRENTadd1;
            sgTOaddress2 = sgCURRENTadd2;
            sgTOaddress3 = sgCURRENTadd3;
            sgTOaddress4 = sgCURRENTadd4;
            sgTOaddress5 = sgCURRENTadd5;
            

            // Labels on the gate
			AddLabel(287, 432, 0, @"Scribe");
			AddLabel(460, 432, 0, @"End");
			AddLabel(373, 432, 0, @"Erase");
			
            this.AddButton(272, 400, 1154, 1155, 2, GumpButtonType.Reply, 0); // Selection 1
            this.AddButton(322, 400, 1154, 1155, 3, GumpButtonType.Reply, 0); // Selection 2
            this.AddButton(372, 400, 1154, 1155, 4, GumpButtonType.Reply, 0); // Selection 3
            this.AddButton(422, 400, 1154, 1155, 5, GumpButtonType.Reply, 0); // Selection 4
            this.AddButton(472, 400, 1154, 1155, 6, GumpButtonType.Reply, 0); // Selection 5
            this.AddButton(273, 450, 247, 248, 7, GumpButtonType.Reply, 0); // ACTIVATE
            this.AddButton(357, 450, 238, 240, 8, GumpButtonType.Reply, 0); // RESET
            this.AddButton(438, 450, 242, 241, 9, GumpButtonType.Reply, 0); // CANCEL

            // Administrator & GM Functions
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                this.AddLabel(370, 328, 0, @"Overseer Functions");
                this.AddButton(299, 327, 247, 248, 11, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            int destIndex = 0; // index of destination
            int fromIndex = 0; // index of origin

            if (!from.InRange(m_sgactivatordevice.GetWorldLocation(), 1) || from.Map != m_sgactivatordevice.Map)
            {
                from.SendMessage(33, "You are too far away to use that.");
                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                from.CloseGump(typeof(SGGumpActivatorDevice));
            }
            else
            {
                // Allow use of the gate

                #region selection1
                if (sgTOselectioncounter == 1)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = 1;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = 2;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = 3;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = 4;
                                sgTOselectioncounter++;

                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = 5;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));

                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar doesn't seem to respond.");

                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "The altar does nothing, it would appear.");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);

                                                    // setup energy bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)
													from.SendMessage("The altar erupts with activity!");

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer HERE
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter

                                                    // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "The altar doesn't seem to respond.");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer HERE
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn the page, and begin anew.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You remove your spellbook from the altar.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "Help Option selected");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
                #endregion selection1

                #region selection2
                else if (sgTOselectioncounter == 2)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress2 = 1;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress2 = 2;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress2 = 3;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress2 = 4;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress2 = 5;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar appears to be in use...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "Nothing seems to happen...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);

                                                    // setup energy bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counters
													
													

                                                   // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }

                                                    
                                                    // check direction of DESTINATION GATE, place energy tiles
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "Nothing seems to happen...");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn to a new page and start over.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "help");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
                #endregion selection2

                #region selection3
                else if (sgTOselectioncounter == 3)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress3 = 1;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress3 = 2;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress3 = 3;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress3 = 4;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress3 = 5;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar doesn't seem to respond.");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "Nothing seems to happen...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);

                                                    // setup energy bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter

                                                    // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "The altar doesn't seem to respond.");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn to a new page and start over.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "help");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
                #endregion selection3

                #region selection4
                else if (sgTOselectioncounter == 4)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress4 = 1;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress4 = 2;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress4 = 3;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress4 = 4;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress4 = 5;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar doesn't seem to respond.");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "Nothing seems to happen...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);

                                                    // setup energy bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter

                                                   // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "The altar doesn't seem to respond.");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn to a new page and start over.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "help");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
                #endregion selection4

                #region selection5
                else if (sgTOselectioncounter == 5)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress5 = 1;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress5 = 2;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress5 = 3;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress5 = 4;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress5 = 5;
                                sgTOselectioncounter++;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar doesn't seem to respond.");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "Nothing seems to happen...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);

                                                    // setup energy bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter

                                                    // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "The altar doesn't seem to respond.");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn to a new page and start over.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "help");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
                #endregion selection5

                #region selection6
                else if (sgTOselectioncounter == 6)
                {
                    switch (info.ButtonID)
                    {
                        case 0:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid + 1;
                                if (sgTOfacetid >= 5)
                                {
                                    sgTOfacetid = 5;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 1:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                sgTOfacetid = sgTOfacetid - 1;
                                if (sgTOfacetid <= 1)
                                {
                                    sgTOfacetid = 1;
                                }
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 2:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                //from.PublicOverheadMessage(MessageType.Whisper, 0, false, "Activate / Reset or Cancel");
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 3:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                //from.PublicOverheadMessage(MessageType.Whisper, 0, false, "Activate / Reset or Cancel");
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 4:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                //from.PublicOverheadMessage(MessageType.Whisper, 0, false, "Activate / Reset or Cancel");
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 5:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                //from.PublicOverheadMessage(MessageType.Whisper, 0, false, "Activate / Reset or Cancel");
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 6:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                               //from.PublicOverheadMessage(MessageType.Whisper, 0, false, "Activate / Reset or Cancel");
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 7:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButtonActivate);
                                from.CloseGump(typeof(SGGumpActivatorDevice));

                                // check I can go from here
                                bool stage1check = false;

                                for (int i = 0; i < SGCore.SGList.Count; i++)
                                {
                                    SGEntry sge = (SGEntry)SGCore.SGList[i];
                                    {
                                        if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                        {
                                            // this gate
                                            fromIndex = i;
                                            if (!sge.SGEnergy)
                                            {
                                                // i can continue
                                                stage1check = true;
                                            }
                                            else
                                            {
                                                from.SendMessage(33, "The altar doesn't seem to respond.");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                        }
                                    }
                                }

                                if (stage1check)
                                {
                                    // find destination
                                    for (int i = 0; i < SGCore.SGList.Count; i++)
                                    {
                                        SGEntry sge = (SGEntry)SGCore.SGList[i];
                                        if (sge.SGFacetCode == sgTOfacetid && sge.SGAddressCode1 == sgTOaddress1 && sge.SGAddressCode2 == sgTOaddress2 && sge.SGAddressCode3 == sgTOaddress3 && sge.SGAddressCode4 == sgTOaddress4 && sge.SGAddressCode5 == sgTOaddress5)
                                        {
                                            // match
                                            if (sge.SGFacetCode == m_sgactivatordevice.SGAFacetNumber && sge.SGAddressCode1 == m_sgactivatordevice.SGAAddressCode1 && sge.SGAddressCode2 == m_sgactivatordevice.SGAAddressCode2 && sge.SGAddressCode3 == m_sgactivatordevice.SGAAddressCode3 && sge.SGAddressCode4 == m_sgactivatordevice.SGAAddressCode4 && sge.SGAddressCode5 == m_sgactivatordevice.SGAAddressCode5)
                                            {
                                                // Already here (Code selected was same code)
                                                from.SendMessage(33, "Nothing seems to happen...");
                                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                            }
                                            else
                                            {
                                                // match and different from where you are
                                                destIndex = i;

                                                sgDESTINATIONlocX = sge.SGX;
                                                sgDESTINATIONlocY = sge.SGY;
                                                sgDESTINATIONlocZ = sge.SGZ;
                                                sgDESTINATIONfacing = sge.SGFacing;

                                                sgDESTINATIONfacet = sge.SGFacetCode;
                                                sgDESTINATIONaddress1 = sge.SGAddressCode1;
                                                sgDESTINATIONaddress2 = sge.SGAddressCode2;
                                                sgDESTINATIONaddress3 = sge.SGAddressCode3;
                                                sgDESTINATIONaddress4 = sge.SGAddressCode4;
                                                sgDESTINATIONaddress5 = sge.SGAddressCode5;

                                                sgDESTINATIONbusyused = sge.SGBeingUsed;
                                                sgDESTINATIONbusyenergy = sge.SGEnergy;

                                                if (!sgDESTINATIONbusyenergy)
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundGoodToGo);
                                                    // No energy field at DESTINATION gate, set bits
                                                    SGEntry sgeDestination = (SGEntry)SGCore.SGList[destIndex];
                                                    sgeDestination.SGEnergy = true; // turn ON energy bit at DESTINATION gate (stops outbound calls)
                                                    SGEntry sgeOrigin = (SGEntry)SGCore.SGList[fromIndex];
                                                    sgeOrigin.SGEnergy = true; // turn ON energy HERE (stops inbounds)

                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter

                                                   // place SGTeleport Tile into doorway TO THE DESTINATION Location (FROM HERE TO x,y,z)
                                                    if (sgDESTINATIONfacet == 1)
                                                    {
                                                        // Tile FROM Here TO Destination
                                                        SGFieldTile SGTileFROM = new SGFieldTile(fromIndex, destIndex, new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                        SGTileFROM.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        // Tile FROM Destination To Here
                                                        SGFieldTile SGTileBACK = new SGFieldTile(fromIndex, destIndex, new Point3D(m_sgactivatordevice.X, m_sgactivatordevice.Y - 4 , m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                        SGTileBACK.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);

                                                        // ORIGIN ENERGY FIELD

                                                            SGCore.SGEffectEastWest(fromIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1 = new SGEFieldEast(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile1.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 6), m_sgactivatordevice.Map);
                                                            SGParts SGEnergyTile2 = new SGParts(m_sgactivatordevice.Location, m_sgactivatordevice.Map);
                                                            SGEnergyTile2.MoveToWorld(new Point3D(m_sgactivatordevice.X -1, m_sgactivatordevice.Y - 4, m_sgactivatordevice.Z + 7), m_sgactivatordevice.Map);

                                                        // DESTINATION ENERGY FIELD

                                                            SGCore.SGEffectEastWest(destIndex);
                                                            // place energy field in doorway
                                                            SGEFieldEast SGEnergyTile1d = new SGEFieldEast(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile1d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 6), Map.Felucca);
                                                            SGParts SGEnergyTile2d = new SGParts(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ), Map.Felucca);
                                                            SGEnergyTile2d.MoveToWorld(new Point3D(sgDESTINATIONlocX, sgDESTINATIONlocY, sgDESTINATIONlocZ + 7), Map.Felucca);

                                                    }
                                                }
                                                else
                                                {
                                                    Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundNoTravel);
                                                    from.SendMessage(33, "The altar doesn't seem to respond.");
                                                    m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                                    m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                                    m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                                }
                                            }
                                        }
                                        m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                        m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                        m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                    }
                                }
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 8:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "You turn to a new page and start over.");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                int sgTOaddress1 = reset;
                                int sgTOaddress2 = reset;
                                int sgTOaddress3 = reset;
                                int sgTOaddress4 = reset;
                                int sgTOaddress5 = reset;
                                sgTOselectioncounter = 1;
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 9:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                            }
                            break;

                        case 10:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.SendMessage(33, "help");
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                from.SendGump(new SGGumpActivatorDevice(from, m_sgactivatordevice, sgTOselectioncounter, sgTOfacetid, sgTOaddress1, sgTOaddress2, sgTOaddress3, sgTOaddress4, sgTOaddress5));
                            }
                            break;

                        case 11:
                            {
                                Effects.PlaySound(m_sgactivatordevice.Location, m_sgactivatordevice.Map, SGCore.SGSystemSoundButton);
                                from.CloseGump(typeof(SGGumpActivatorDevice));
                                m_sgactivatordevice.m_SGABeingUsed = false; // release dialer
                                m_sgactivatordevice.m_WhoClickedIt = "Blank"; // Reset Who clicked it value
                                m_sgactivatordevice.m_TimerCounter = 0; // Reset Timer Counter
                                from.SendGump(new SGGumpActivatorRemoval(m_sgactivatordevice));
                            }
                            break;
                    }
                }
            }
            #endregion selection6
        }
    }

    public class SGParts : SGEnergyParticles
    {
        public SGParts(Point3D from, Map map) : base(from, map)
        {
            Map = map;
            InternalTimer t = new InternalTimer(this);
            t.Start();
        }

        public SGParts(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }

        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item item) : base(TimeSpan.FromSeconds(SGCore.SGSystemGateTime))
            {
                Priority = TimerPriority.OneSecond;
                m_Item = item;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }
	
	    public class SGEFieldEast : SGEnergyFieldEast
    {
        public SGEFieldEast(Point3D from, Map map) : base(from, map)
        {
            Map = map;
            InternalTimer t = new InternalTimer(this);
            t.Start();
        }
		
	    public SGEFieldEast(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }

        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item item) : base(TimeSpan.FromSeconds(SGCore.SGSystemGateTime))
            {
                Priority = TimerPriority.OneSecond;
                m_Item = item;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }

    public class SGEFieldSouth : SGEnergyFieldSouth
    {
        public SGEFieldSouth(Point3D from, Map map) : base(from, map)
        {
            Map = map;
            InternalTimer t = new InternalTimer(this);
            t.Start();
        }

        public SGEFieldSouth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }

        private class InternalTimer : Timer
        {
            private Item m_Item;

            public InternalTimer(Item item) : base(TimeSpan.FromSeconds(SGCore.SGSystemGateTime))
            {
                Priority = TimerPriority.OneSecond;
                m_Item = item;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }

    public class SGFieldTile : SGTransportTile
    {
        private int m_FromID;
        private int m_ToID;

        public int FromID
        {
            get { return m_FromID; }
            set { m_FromID = value; InvalidateProperties(); }
        }

        public int ToID
        {
            get { return m_ToID; }
            set { m_ToID = value; InvalidateProperties(); }
        }

        public SGFieldTile(int frm, int des, Point3D from, Map map) : base(from, map)
        {
            m_FromID = frm;
            m_ToID = des;
            Map = map;
            InternalTimer t = new InternalTimer(this, this);
            t.Start();
        }

        public SGFieldTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            Delete();
        }

        public class InternalTimer : Timer
        {
            private SGFieldTile m_sgfieldtile;
            private Item m_Item;

            public InternalTimer(SGFieldTile m, Item item) : base(TimeSpan.FromSeconds(SGCore.SGSystemGateTime))
            {
                m_sgfieldtile = m;
                m_Item = item;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                SGEntry sgorigin = (SGEntry)SGCore.SGList[m_sgfieldtile.FromID];
                SGEntry sgdest = (SGEntry)SGCore.SGList[m_sgfieldtile.ToID];

                sgorigin.SGEnergy = false;
                sgdest.SGEnergy = false;

                m_Item.Delete();
            }
        }
    }
}