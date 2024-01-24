﻿using Robot.CommandModels;
using GLaDOS.Service.SoundSpeaker;

namespace GLaDOS.VoiceMode.Model
{
    public class CommandCommentPair
    {
        public CommandCommentPair(IGladCommand command, Speaker.Audio audio)
        {
            Command = command;
            Audio = audio;
        }

        public IGladCommand Command { get; set; }
        public Speaker.Audio Audio { get; set; }
    }

    public class UndefinedCommandComment : CommandCommentPair
    {
        public UndefinedCommandComment() : base(
            new PhysCommand(PhysCommand.DeviceType.Animation, 1, 0),
            Speaker.Audio.Beatles
            )
        { }
    }
}
 