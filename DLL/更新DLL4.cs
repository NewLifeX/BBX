// 自动选择最新的文件源
var srcs = new String[] { @"..\Bin4", @"C:\X\DLL4", @"C:\X\Bin4", @"E:\X\DLL4", @"E:\X\Bin4" };
var cur = ".".GetFullPath();
foreach (var item in srcs)
{
    // 跳过当前目录
    if (item.EqualIgnoreCase(cur)) continue;

    Console.WriteLine("复制 {0} => {1}", item, cur);

    try
    {
        item.AsDirectory().CopyToIfNewer(cur, "*.dll;*.exe;*.xml;*.pdb;*.cs", false,
            name => Console.WriteLine("\t{1}\t{0}", name, item.CombinePath(name).AsFile().LastWriteTime.ToFullString()));
    }
    catch (Exception ex) { Console.WriteLine(" " + ex.Message); }
}