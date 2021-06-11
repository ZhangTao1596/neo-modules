using System;
using Neo.Consensus;
using Neo.ConsoleService;
using Neo.Network.P2P;
using Neo.Network.P2P.Payloads;

namespace Neo.Plugins.P2P
{
    public class P2PFilterPlugin : Plugin, IP2PPlugin
    {
        private bool filterResponse = false;
        private bool filterRequest = false;

        public bool OnP2PMessage(NeoSystem system, Message message)
        {
            if (message.Command == MessageCommand.Extensible)
            {
                var payload = (ExtensiblePayload)message.Payload;
                if (payload.Category == "dBFT")
                {
                    var consensus = ConsensusMessage.DeserializeFrom(payload.Data);
                    if (consensus.Type == ConsensusMessageType.PrepareResponse && filterResponse)
                        return false;
                    if (consensus.Type == ConsensusMessageType.PrepareRequest && filterRequest)
                        return false;
                }
            }
            return true;
        }

        [ConsoleCommand("resp off", Category = "P2PFilter", Description = "Filter p2p messages")]
        private void ResponseOff()
        {
            filterResponse = true;
            Console.WriteLine("Response Off. Won't handle PrepareResponse.");
        }

        [ConsoleCommand("req off", Category = "P2PFilter", Description = "Filter p2p messages")]
        private void RequestOff()
        {
            filterRequest = true;
            Console.WriteLine("Request Off. Won't handle PrepareRequest.");
        }

        [ConsoleCommand("resp on", Category = "P2PFilter", Description = "Filter p2p messages")]
        private void ResponseOn()
        {
            filterResponse = false;
            Console.WriteLine("Response On. Consume handle PrepareResponse.");
        }

        [ConsoleCommand("req on", Category = "P2PFilter", Description = "Filter p2p messages")]
        private void RequestOn()
        {
            filterRequest = false;
            Console.WriteLine("Request On. Consume handle PrepareRequest.");
        }
    }
}
