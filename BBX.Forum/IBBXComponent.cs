using System;
using System.ComponentModel;
namespace BBX.Forum
{
	public interface IBBXComponent : IComponent, IDisposable
	{
		string ComponentName { get; set; }
		string InnerName { get; set; }
	}
}