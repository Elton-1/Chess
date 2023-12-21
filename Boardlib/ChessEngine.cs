using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class ChessEngine
{
    private Process stockfishProcess;
    string EnginePath = String.Empty;

    public ChessEngine(string enginePath)
    {
        EnginePath = enginePath;
    }

    public string SendCommand(string command)
    {
        if (stockfishProcess is null) throw new Exception();

        stockfishProcess.StandardInput.WriteLine(command);
        stockfishProcess.StandardInput.Flush();
        return stockfishProcess.StandardOutput.ReadLine();
    }

    public async Task<string> GetBestMove(String fen, int think = 100)
    {
        //Start the process
        stockfishProcess = new Process();
        stockfishProcess.StartInfo.FileName = EnginePath;
        stockfishProcess.StartInfo.UseShellExecute = false;
        stockfishProcess.StartInfo.RedirectStandardInput = true;
        stockfishProcess.StartInfo.RedirectStandardOutput = true;
        stockfishProcess.StartInfo.CreateNoWindow = true;
        stockfishProcess.Start();

        SendCommand($"position fen {fen}");
        SendCommand($"go movetime {think}"); // Start the engine thinking

        // Keep reading the output until you find the best move or another relevant line
        while (true)
        {
            string line = await stockfishProcess.StandardOutput.ReadLineAsync();
            if (line != null)
            {
                if (line.StartsWith("bestmove"))
                {
                    Dispose();
                    return line.Split(' ')[1]; // Extract the best move
                }
            }
        }
    }


    public void Dispose()
    {
        if (stockfishProcess is null) return;

        SendCommand("quit");
        stockfishProcess.WaitForExit();
        stockfishProcess.Close();
    }
}