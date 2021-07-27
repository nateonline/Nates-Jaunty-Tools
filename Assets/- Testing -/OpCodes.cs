public enum OpCode
{
	ChatMessage = 1,
}

public static class OpCodeExtensions
{
	public static byte ToByte(this OpCode c) { return (byte)c; }

	public static OpCode ToOpCode (this byte b) { return (OpCode)b; }
}