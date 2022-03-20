using System;
using System.Threading;
using System.Threading.Tasks;

namespace RawBot.Runtime.Scripts
{
    public class ScriptManager
    {
        private Script _script;
        private Thread _scriptThread;

        public bool Running => _scriptThread?.IsAlive ?? false;

        public string CurrentScriptName { get; set; }
        public ScriptException RuntimeException { get; set; }

        public void Load(string name, string source)
        {
            if (_script is not null)
            {
                throw new InvalidOperationException("Cannot load script while another script is already loaded.");
            }

            CurrentScriptName = name;
            _script = new Script(source);
            _script.Compile();
        }

        public void Start(Context context)
        {
            RequireScript();

            _scriptThread = new Thread(() => StartScript(context));
            _scriptThread.Start();
        }

        private async void StartScript(Context context)
        {
            RuntimeException = null;
            try
            {
                await _script.Invoke(context);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception e)
            {
                RuntimeException = new ScriptException(e);
            }
        }

        public void Stop()
        {
            RequireScript();

            _script.Cancel();
            _scriptThread.Join();
            _script.Dispose();
            _script = null;
            _scriptThread = null;
            CurrentScriptName = null;
        }

        private void RequireScript()
        {
            if (_script is null)
            {
                throw new InvalidOperationException("No script loaded.");
            }
        }
    }
}
