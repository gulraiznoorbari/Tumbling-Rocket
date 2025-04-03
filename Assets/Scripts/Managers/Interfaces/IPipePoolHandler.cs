using System.Collections.Generic;

public interface IPipePoolHandler
{
    public List<Pipe> GetAllPipes();
    public Pipe GetPipe();
    public Pipe GetMostRecentPipe();
}