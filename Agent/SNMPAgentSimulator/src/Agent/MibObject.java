package Agent;

public class MibObject {

    public enum SNMPType{Integer32, OctetString};

    public String Id;
    public SNMPType Type;
    public Object Value;

    public MibObject(String id, SNMPType type, Object value)
    {
        Id = id;
        Type = type;
        this.Value = value;
    }
}
