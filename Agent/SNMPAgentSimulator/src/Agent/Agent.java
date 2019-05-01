package Agent;

/**
 * Created by: Péter Sturm
 * Date: 2019-03-23
 */

import org.snmp4j.CommunityTarget;
import org.snmp4j.PDU;
import org.snmp4j.ScopedPDU;
import org.snmp4j.UserTarget;
import org.snmp4j.agent.*;
import org.snmp4j.agent.example.SampleAgent;
import org.snmp4j.agent.example.Snmp4jDemoMib;
import org.snmp4j.agent.io.ImportModes;
import org.snmp4j.agent.mo.*;
import org.snmp4j.agent.mo.ext.AgentppSimulationMib;
import org.snmp4j.agent.mo.snmp.*;
import org.snmp4j.agent.mo.snmp4j.example.Snmp4jHeartbeatMib;
import org.snmp4j.agent.security.MutableVACM;
import org.snmp4j.log.LogAdapter;
import org.snmp4j.log.LogFactory;
import org.snmp4j.mp.MPv3;
import org.snmp4j.mp.MessageProcessingModel;
import org.snmp4j.mp.SnmpConstants;
import org.snmp4j.security.*;
import org.snmp4j.security.nonstandard.PrivAES256With3DESKeyExtension;
import org.snmp4j.smi.*;
import org.snmp4j.transport.DefaultUdpTransportMapping;


import java.io.File;
import java.io.IOException;
import java.util.*;

public class Agent  extends BaseAgent {

    private static final LogAdapter logger =
            LogFactory.getLogger(BaseAgent.class);
    public String address;
    public int port;
    public String trapListenerAddress;
    private OctetString trapUserName;
    private OctetString trapAuth;
    private OctetString trapPriv;
    private UserTarget target;
    private Snmp4jHeartbeatMib heartbeatMIB;
    private OctetString localEngineID;

    public void setMibObjects(ArrayList<MibObject> mibObjects) {
        this.mibObjects = mibObjects;
    }

    private ArrayList<MibObject> mibObjects;

    public Agent(File bootCounterFile, File configFile)
    {
        super(bootCounterFile, configFile,
                new CommandProcessor((new OctetString(MPv3.createLocalEngineID()).substring(0,9))));
        SecurityProtocols.getInstance().addPrivacyProtocol(new PrivAES256With3DESKeyExtension());

        localEngineID = agent.getContextEngineID();
    }

    public void start(String address, int port, String trapAddress, String trapusername, String trapaut, String trappriv)
    {
        if (address == null || address == "")
            address = "127.0.0.1";

        address += "/" + String.format("%d", port);
        this.port = port;

        trapListenerAddress = trapAddress;
        trapUserName = new OctetString(trapusername);
        trapAuth = new OctetString(trapaut);
        trapPriv = new OctetString(trappriv);

        try {
            this.address = address;
            init();
            configTrap();
            addShutdownHook();
            getServer().addContext(new OctetString("public"));
            finishInit();

            run();
            sendColdStartNotification();
        }
        catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    private void configTrap()
    {
        target = new UserTarget();
        target.setVersion(SnmpConstants.version3);
        target.setSecurityLevel(SecurityLevel.AUTH_PRIV);
        target.setSecurityName(trapUserName);
        target.setAddress(new UdpAddress(trapListenerAddress));
        session.setLocalEngine(localEngineID.getValue(), 0, 0);
    }

    public void sendTrap()
    {
        ScopedPDU pdu = new ScopedPDU();
        pdu.setType(PDU.TRAP);
        pdu.setContextEngineID(localEngineID);
        //pdu.add(new VariableBinding(SnmpConstants.sysUpTime, new TimeTicks(5632)));
        //pdu.add(new VariableBinding(SnmpConstants.snmpTrapOID, new OID(".1.3.6.1.2.1.1.8")));
        pdu.add(new VariableBinding(new OID(".1.3.6.1.2.1.1.8"), new OctetString("hearbeat")));
        //Collection<ManagedObject> mos = server.getRegistry().values();
        /*pdu.add(new VariableBinding(new OID(heartbeatMIB.oidSnmp4jAgentHBRefTime), heartbeatMIB.getSnmp4jAgentHBCtrlEntry().getValue(new OID(heartbeatMIB.oidSnmp4jAgentHBRefTime))));
        ManagedObject mo = server.getManagedObject(new OID("1.3.6.1.2.1.2.2.1"), null);*/

        try {
            session.send(pdu, target);
            System.out.println(address + " sent trap to: " + trapListenerAddress);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    protected void registerSnmpMIBs() {
        /*heartbeatMIB = new Snmp4jHeartbeatMib(super.getNotificationOriginator(),
                new OctetString(),
                super.snmpv2MIB.getSysUpTime());*/
        super.registerSnmpMIBs();
    }

    @Override
    protected void initTransportMappings() throws IOException {
        transportMappings = new DefaultUdpTransportMapping[] {
                new DefaultUdpTransportMapping(new UdpAddress("0.0.0.0/"+ String.format("%d", port))) };
    }

    @Override
    protected void registerManagedObjects() {
        try {
            //server.register(createGPSTable(4700000, 4500000, 100), null);

            if(mibObjects != null)
                server.register(createGPSTable(mibObjects), null);
        }
        catch (DuplicateRegistrationException ex) {
            ex.printStackTrace();
        }
    }

    @Override
    protected void unregisterManagedObjects() {

    }

    @Override
    protected void addUsmUser(org.snmp4j.security.USM usm) {
        UsmUser user = new UsmUser(new OctetString("admin"),
                AuthSHA.ID,
                new OctetString("authpass012"),
                PrivDES.ID,
                new OctetString("privpass012"));

        UsmUser trapuser = new UsmUser(trapUserName
                ,AuthSHA.ID
                ,trapAuth
                ,PrivDES.ID
                ,trapPriv);

        usm.addUser(user.getSecurityName(), null, user);
        usm.addUser(trapuser.getSecurityName(), null, trapuser);
    }

    @Override
    protected void addNotificationTargets(SnmpTargetMIB snmpTargetMIB, SnmpNotificationMIB snmpNotificationMIB) {
        snmpTargetMIB.addDefaultTDomains();

        snmpTargetMIB.addTargetAddress(new OctetString("notification"),
                TransportDomains.transportDomainUdpIpv4,
                new OctetString(new UdpAddress(trapListenerAddress).getValue()),
                200, 1,
                new OctetString("notify"),
                new OctetString("v3"),
                StorageType.permanent);
        snmpTargetMIB.addTargetParams(new OctetString("v3"),
                MessageProcessingModel.MPv3,
                SecurityModel.SECURITY_MODEL_USM,
                new OctetString("public"),
                SecurityLevel.NOAUTH_NOPRIV,
                StorageType.permanent);
        snmpNotificationMIB.addNotifyEntry(new OctetString("default"),
                new OctetString("notify"),
                SnmpNotificationMIB.SnmpNotifyTypeEnum.trap,
                StorageType.permanent);
    }

    @Override
    protected void addViews(VacmMIB vacmMIB) {
        vacmMIB.addGroup(SecurityModel.SECURITY_MODEL_USM,
                new OctetString("admin"),
                new OctetString("allagroup"),
                StorageType.nonVolatile);

        vacmMIB.addAccess(new OctetString("allagroup"), new OctetString(),
                SecurityModel.SECURITY_MODEL_USM,
                SecurityLevel.AUTH_PRIV,
                MutableVACM.VACM_MATCH_EXACT,
                new OctetString("fullReadView"),
                new OctetString("fullWriteView"),
                new OctetString("fullNotifyView"),
                StorageType.nonVolatile);

        vacmMIB.addViewTreeFamily(new OctetString("fullReadView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullWriteView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullNotifyView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
    }

    @Override
    protected void addCommunities(SnmpCommunityMIB snmpCommunityMIB) {
        Variable[] com2sec = new Variable[] {
                new OctetString("public"),              // community name
                new OctetString("admin"),              // security name
                getAgent().getContextEngineID(),        // local engine ID
                new OctetString("public"),              // default context name
                new OctetString(),                      // transport tag
                new Integer32(StorageType.nonVolatile), // storage type
                new Integer32(RowStatus.active)         // row status
        };
        SnmpCommunityMIB.SnmpCommunityEntryRow row =
                snmpCommunityMIB.getSnmpCommunityEntry().createRow(
                        new OctetString("public2public").toSubIndex(true), com2sec);
        snmpCommunityMIB.getSnmpCommunityEntry().addRow(row);
    }

    private DefaultMOTable createGPSTable(int lat, int lon, int elv)
    {
        // Create table -->
        MOTableSubIndex[] subIndexes = new MOTableSubIndex[] {
                new MOTableSubIndex(SMIConstants.SYNTAX_INTEGER)
        };
        MOTableIndex indexDef = new MOTableIndex(subIndexes, false);

        MOColumn[] columns = new MOColumn[1];
        int columnIndex = 0;
        columns[columnIndex++] = DefaultMOFactory.getInstance().createColumn(8, SMIConstants.SYNTAX_INTEGER32, MOAccessImpl.ACCESS_READ_WRITE);

        DefaultMOTable table = new DefaultMOTable(new OID("0.1.15628.4.1"), indexDef, columns);
        // <-- Create table

        // Create Rows -->
        DefaultMOTableModel<DefaultMOTableRow> model = (DefaultMOTableModel<DefaultMOTableRow>) table.getModel();
        Variable[] row1at = new Variable[]
                {
                        new Integer32(lat),
                };
        Variable[] row1on = new Variable[]
                {
                        new Integer32(lon)
                };
        Variable[] rowelv = new Variable[]
                {
                        new Integer32(elv)
                };
        model.addRow(new DefaultMOMutableRow2PC(new OID("6"), row1at));
        model.addRow(new DefaultMOMutableRow2PC(new OID("7"), row1on));
        model.addRow(new DefaultMOMutableRow2PC(new OID("8"), rowelv));
        // <-- Create Rows

        table.setVolatile(true);
        return table;
    }

    private DefaultMOTable createGPSTable(ArrayList<MibObject> mibObjects)
    {
        // Create table -->
        MOTableSubIndex[] subIndexes = new MOTableSubIndex[] {
                new MOTableSubIndex(SMIConstants.SYNTAX_INTEGER)
        };
        MOTableIndex indexDef = new MOTableIndex(subIndexes, false);

        MOColumn[] columns = new MOColumn[1];
        int columnIndex = 0;
        columns[columnIndex++] = DefaultMOFactory.getInstance().createColumn(8, SMIConstants.SYNTAX_INTEGER32, MOAccessImpl.ACCESS_READ_WRITE);

        DefaultMOTable table = new DefaultMOTable(new OID("0.1.15628.4.1"), indexDef, columns);
        // <-- Create table

        // Create Rows -->
        DefaultMOTableModel<DefaultMOTableRow> model = (DefaultMOTableModel<DefaultMOTableRow>) table.getModel();

        ArrayList<Variable[]> rows = new ArrayList<Variable[]>();
        for(MibObject obj : mibObjects)
        {
            switch (obj.Type)
            {
                case Integer32:
                    model.addRow(new DefaultMOMutableRow2PC(new OID(obj.Id), new Variable[]
                            {
                                    new Integer32((int)obj.value)
                            }));
                    break;
                case OctetString:
                    model.addRow(new DefaultMOMutableRow2PC(new OID(obj.Id), new Variable[]
                            {
                                    new OctetString((String) obj.value)
                            }));
                    break;
            }
        }
        // <-- Create Rows

        table.setVolatile(true);
        return table;
    }

    private static DefaultMOTable createStaticIfTable() {
        MOTableSubIndex[] subIndexes =
                new MOTableSubIndex[] { new MOTableSubIndex(SMIConstants.SYNTAX_INTEGER) };
        MOTableIndex indexDef = new MOTableIndex(subIndexes, false);
        MOColumn[] columns = new MOColumn[8];
        int c = 0;
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifIndex
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_OCTET_STRING,
                        MOAccessImpl.ACCESS_READ_ONLY);// ifDescr
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifType
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifMtu
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_GAUGE32,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifSpeed
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_OCTET_STRING,
                        MOAccessImpl.ACCESS_READ_ONLY);// ifPhysAddress
        columns[c++] =
                new MOMutableColumn(c, SMIConstants.SYNTAX_INTEGER,     // ifAdminStatus
                        MOAccessImpl.ACCESS_READ_WRITE, null);
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifOperStatus

        DefaultMOTable ifTable =
                new DefaultMOTable(new OID("1.3.6.1.2.1.2.2.1"), indexDef, columns);
        MOMutableTableModel model = (MOMutableTableModel) ifTable.getModel();
        Variable[] rowValues1 = new Variable[] {
                new Integer32(1),
                new OctetString("eth0"),
                new Integer32(6),
                new Integer32(1500),
                new Gauge32(100000000),
                new OctetString("00:00:00:00:01"),
                new Integer32(1),
                new Integer32(1)
        };
        Variable[] rowValues2 = new Variable[] {
                new Integer32(2),
                new OctetString("loopback"),
                new Integer32(24),
                new Integer32(1500),
                new Gauge32(10000000),
                new OctetString("00:00:00:00:02"),
                new Integer32(1),
                new Integer32(1)
        };
        model.addRow(new DefaultMOMutableRow2PC(new OID("1"), rowValues1));
        model.addRow(new DefaultMOMutableRow2PC(new OID("2"), rowValues2));
        ifTable.setVolatile(true);
        return ifTable;

    }
}
