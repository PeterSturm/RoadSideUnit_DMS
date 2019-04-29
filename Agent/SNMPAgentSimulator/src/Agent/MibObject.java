package Agent;

public class MibObject {

    public enum SNMPType{Integer32, OctetString};

    public String Id;
    public SNMPType Type;
    public Object value;
}
