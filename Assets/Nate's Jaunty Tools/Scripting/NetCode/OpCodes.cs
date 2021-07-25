/**/
public enum OpCode
{
	ChatMessage = 1,
	PlayerPosition = 2,
}

public static class OpCodeExtensions
{
	public static byte ToByte(this OpCode opCode) { return (byte)opCode; }

	public static OpCode ToOpCode(this byte b) { return (OpCode)b; }
}
/**/


/**
public static class OpCode
{
	public const byte ChatMessage = 1;
	public const byte PlayerPosition = 2;
}
/**/
