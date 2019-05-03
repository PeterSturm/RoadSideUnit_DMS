using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using SNMPManager.Core.Exceptions;

namespace SNMPManager.Infrastructure
{
   
    public class SNMPManagerService : ISNMPManagerService
    {
        private ManagerSettings _managerSettings;

        public SNMPManagerService()
        {

        }
        public SNMPManagerService(ManagerSettings managerSettings)
        {
            _managerSettings = managerSettings;
        }

        public void Configure(ManagerSettings settings)
        {
            _managerSettings = settings;
        }

        public List<MIBObject> Get(RSU rsu, Core.Entities.User user, string OID)
        {
            List<MIBObject> mibObjects;

            try
            {
                IPEndPoint receiver = new IPEndPoint(rsu.IP, rsu.Port);
                int timeout = _managerSettings.Timeout;

                Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
                ReportMessage report = discovery.GetResponse(timeout, receiver);

                var auth = new SHA1AuthenticationProvider(new Lextm.SharpSnmpLib.OctetString(user.SNMPv3Auth));
                var priv = new DESPrivacyProvider(new Lextm.SharpSnmpLib.OctetString(user.SNMPv3Priv), auth);

                GetRequestMessage request = new GetRequestMessage(VersionCode.V3
                    , Messenger.NextMessageId
                    , Messenger.NextRequestId
                    , new OctetString(user.UserName)
                    , new OctetString(String.Empty)
                    , new List<Variable> { new Variable(new ObjectIdentifier(OID)) }
                    , priv
                    , Messenger.MaxMessageSize
                    , report);

                ISnmpMessage reply = request.GetResponse(timeout, receiver);

                // Need to send again (RFC 3414)???
                if (reply is ReportMessage)
                {
                    //throw new ReplyIsReportMessage();
                    request = new GetRequestMessage(VersionCode.V3
                                                    , Messenger.NextMessageId
                                                    , Messenger.NextRequestId
                                                    , new OctetString(user.UserName)
                                                    , new OctetString(String.Empty)
                                                    , new List<Variable> { new Variable(new ObjectIdentifier(OID)) }
                                                    , priv
                                                    , Messenger.MaxMessageSize
                                                    , reply);

                    reply = request.GetResponse(timeout, receiver);
                    if (reply.Pdu().ErrorStatus.ToInt32() != 0)
                        throw new SnmpGetError();
                }
                else if (reply.Pdu().ErrorStatus.ToInt32() != 0)
                    throw new SnmpGetError();

                mibObjects = SNMPVariables2MIBObjects(reply.Pdu().Variables);

                return mibObjects;
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException ex)
            {
                mibObjects = new List<MIBObject>();
                mibObjects.Add(new MIBObject("0", SnmpType.OctetString, "Timeout"));
                return mibObjects;
            }

        }

        public bool Set(RSU rsu, Core.Entities.User user, string OID, SnmpType type, string value)
        {
            IPEndPoint receiver = new IPEndPoint(rsu.IP, rsu.Port);
            int timeout = _managerSettings.Timeout;

            Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
            ReportMessage report = discovery.GetResponse(timeout, receiver);

            var auth = new SHA1AuthenticationProvider(new Lextm.SharpSnmpLib.OctetString(user.SNMPv3Auth));
            var priv = new DESPrivacyProvider(new Lextm.SharpSnmpLib.OctetString(user.SNMPv3Priv), auth);

            ISnmpData data;
            try
            {
                data = ConvertStringValue2SnmpData(type, value);
            }
            catch (InvalidDataType invalidDataType){ throw invalidDataType; }
            catch (FormatException formatException) { throw formatException; }

            List<Variable> variables = new List<Variable>() {
                new Variable(new ObjectIdentifier(OID), data)
            };

            SetRequestMessage request = new SetRequestMessage(VersionCode.V3
                , Messenger.NextMessageId
                , Messenger.NextRequestId
                , new OctetString(user.UserName)
                , new OctetString(String.Empty)
                , variables
                , priv
                , Messenger.MaxMessageSize
                , report);

            ISnmpMessage reply = request.GetResponse(timeout, receiver);

            // Need to send again (RFC 3414)
            if (reply is ReportMessage)
            {
                //throw new ReplyIsReportMessage();
                request = new SetRequestMessage(VersionCode.V3
                , Messenger.NextMessageId
                , Messenger.NextRequestId
                , new OctetString(user.UserName)
                , new OctetString(String.Empty)
                , variables
                , priv
                , Messenger.MaxMessageSize
                , reply);

                reply = request.GetResponse(timeout, receiver);
                if (reply.Pdu().ErrorStatus.ToInt32() != 0)
                    throw new InvalidDataType();
            }
            else if (reply.Pdu().ErrorStatus.ToInt32() != 0)
                throw new InvalidDataType();

            return true;
        }

        public void SetTrapListener(RSU rsu, IPAddress listenerIP, int listenerPort)
        {
            throw new NotImplementedException();
        }

        private List<MIBObject> SNMPVariables2MIBObjects(IList<Variable> varibales)
        {
            List<MIBObject> mibos = new List<MIBObject>();
            foreach (Variable v in varibales)
            {
                /*switch (v.Data.TypeCode)
                {
                    case SnmpType.Integer32:
                    case SnmpType.Counter32:
                    case SnmpType.Gauge32:
                    case SnmpType.Unsigned32:
                    case SnmpType.TimeTicks:
                        mibo.Value = Int32.Parse(v.Data.ToString());
                        break;
                    case SnmpType.OctetString:
                        mibo.Value = v.Data.ToString();
                        break;
                    case SnmpType.IPAddress:
                        mibo.Value = IPAddress.Parse(v.Data.ToString());
                        break;
                    default:
                        mibo.Value = v.Data;
                        break;
                }*/

                mibos.Add(new MIBObject(v.Id.ToString()
                                        , v.Data.TypeCode
                                        , v.Data.ToString() ));
            }

            return mibos;

        }

        private ISnmpData ConvertStringValue2SnmpData(SnmpType type, string value)
        {
            ISnmpData data;

            try
            {
                switch (type)
                {
                    case SnmpType.Integer32:
                        data = new Integer32(int.Parse(value));
                        break;
                    case SnmpType.Counter32:
                        data = new Counter32(uint.Parse(value));
                        break;
                    case SnmpType.Gauge32:
                        data = new Gauge32(uint.Parse(value));
                        break;
                    case SnmpType.TimeTicks:
                        data = new TimeTicks(uint.Parse(value));
                        break;
                    case SnmpType.OctetString:
                        data = new OctetString(value);
                        break;
                    case SnmpType.IPAddress:
                        data = new IP((IPAddress.Parse(value)).GetAddressBytes());
                        break;
                    default:
                        throw new InvalidDataType();
                }
            }
            catch (InvalidCastException)
            {
                throw new InvalidDataType();
            }

            return data;
        }
    }
}
