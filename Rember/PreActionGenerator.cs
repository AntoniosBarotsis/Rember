﻿namespace Rember;

public class PreActionGenerator
{
    public PreActionGenerator(BuildTool buildTool, Type type)
    {
        Text = "";
        BuildTool = buildTool;
        Type = type;
    }

    private string Text { get; set; }
    private BuildTool BuildTool { get; }
    private Type Type { get; }

    public PreActionGenerator AddBuildScript()
    {
        Generate(Events.Build);
        return this;
    }

    public PreActionGenerator AddTestScript()
    {
        Generate(Events.Tests);
        return this;
    }

    // TODO Ask user if they want to build/test instead of enforcing it every time.
    // TODO Ask user if they want output or not
    private void Generate(Events events)
    {
        var command = events == Events.Build ? BuildTool.Build : BuildTool.Test;
        var shebang = Text == "" ? "#!/bin/sh" : "";
        var inputName = events.ToString() + "Input";
        
        var res = @$"{shebang}
exec < /dev/tty

echo """"

echo ""Do you want to run {events.ToString()}? [Y/n]""
read {inputName}

if [ -z ${inputName} ] || [ ${inputName} = ""Y"" ] || [ ${inputName} = ""y"" ]
then
    echo ""==============""
    echo ""Running {events.ToString()}""
    {command} &> /dev/null
    status=$?

    if [ $status -eq 1 ]
    then
        echo ""Build failed, exiting...""
    exit $status
        fi

    echo ""{events.ToString()} passed!""
    echo ""==============""
    echo """"
    echo """"  
fi
";

        Text += res;
    }

    public void WriteToFile()
    {
        var path = Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}";
        using var sw = new StreamWriter(File.Open(path, FileMode.Create));
        sw.Write(Text);
    }
}

public enum Events
{
    Build,
    Tests
}

public enum Type
{
    Commit,
    Push
}