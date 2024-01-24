using Robot.CommandModels;
using GLaDOS.Service.SoundSpeaker;
using GLaDOS.VoiceMode.Model;

namespace GLaDOS.VoiceMode.Handler
{
    public class CommandDefiner
    {
        private IEnumerable<KeyWordsCommandPair> _commandList = new List<KeyWordsCommandPair>()
       {
        new LightOn(),
        new LightOff(),
        new EyeOn(),
        new EyeOff(),
        new ShowItSelf(),
        new TellJoke1(),
        new TellFact1(),
        new Dance1(),
       };
        public CommandDefiner() { }

        public bool Define(string text, out IGladCommand command)
        {
            command = default;
            text = text.ToLower();
           // if(new ActivateWordNoCommand().IsCommandDefinedIn(text)==false) { return false; }


            CommandCommentPair resultPair = new UndefinedCommandComment();
            foreach(var pair in _commandList)
            {
                if (pair.IsCommandDefinedIn(text))
                {
                    resultPair = pair.Pair;
                    break;
                }
            }
            command=resultPair.Command;
            _ = Task.Run(() => { SideActionFor(resultPair.Audio); });
            return true;
        }

        private void SideActionFor(Speaker.Audio audio)
        {
            new Speaker().Play(audio);
        }
    }
}
