using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yahtzee.Logic;
using Yahtzee.Logic.Events;
using Yahtzee.UI.Helpers;

namespace Yahtzee.UI.Views;

public partial class MainForm : Window
{
    private static readonly List<string> SecondSectionButtons = new()
    {
        "ThreeOfAKindBtn",
        "FourOfAKindBtn",
        "FullHouseBtn",
        "SmStraightBtn",
        "LgStraightBtn",
        "YahtzeeBtn",
        "ChanceBtn"
    };

    private readonly List<Image> _diceImages = new();
    private readonly YahtzeeProcessor _yahtzeeProcessor = new();

    private int _rollsLeftForStep;

    public MainForm()
    {
        InitializeComponent();
        GameStartComponentsSetup();
    }

    private void PcStepHandler(object? sender, PcStepDoneEventArgs e)
    {
        var label = FindName($"Pc{e.YahtzeeStep}Lbl") as Label;
        if (label is null)
        {
            Throw.InvalidUiElementCast($"Pc{e.YahtzeeStep}Lbl", typeof(Label));
        }

        label.Content = e.Points;

        if (e.YahtzeeStep is YahtzeeStep.Yahtzee)
        {
            PcYahtzeeBonusLbl.Content = e.YahtzeeBonus!.Value;
        }
        ReplaceDiceImages();
    }

    private void SectionCompleteHandler(object? sender, SectionCompleteEventArgs e)
    {
        if (e.IsHuman)
        {
            switch (e.SectionNumber)
            {
                case 1:
                    EnableSecondSectionButtons(true);

                    FirstSectionTotalLbl.Content = e.SectionTotal;
                    FirstSectionBonusLbl.Content = e.SectionBonus;
                    break;
                case 2:
                    SecondSectionTotalLbl.Content = e.SectionTotal;
                    break;
            }
        }
        else
        {
            switch (e.SectionNumber)
            {
                case 1:
                    PcFirstSectionTotalLbl.Content = e.SectionTotal;
                    PcFirstSectionBonusLbl.Content = e.SectionBonus;
                    break;
                case 2:
                    PcSecondSectionTotalLbl.Content = e.SectionTotal;
                    break;
            }
        }
    }

    private void GameStartComponentsSetup()
    {
        _yahtzeeProcessor.PcStepDone += PcStepHandler;
        _yahtzeeProcessor.SectionComplete += SectionCompleteHandler;
        _yahtzeeProcessor.GameOver += GameOverHandler;
        _rollsLeftForStep = 3;

        for (var i = 0; i < 5; i++)
        {
            _diceImages.Add((FindName($"DieImg{i}") as Image)!);
        }

        EnableSecondSectionButtons(false);
    }

    private void GameOverHandler(object? sender, GameOverEventArgs e)
    {
        MessageBox.Show(this, "Game over", "Game over");
    }

    private void DieImg_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var dieImage = sender as Image;
        if (dieImage is null)
        {
            Throw.InvalidUiElementCast(sender, typeof(Image));
        }

        if (dieImage.Source is null)
        {
            return;
        }

        var dieIndex = int.Parse(dieImage.Uid[2..]);

        var die = _yahtzeeProcessor.Dice[dieIndex];
        die.IsHold = !die.IsHold;

        var dieArea = FindName($"DieImg{dieIndex}Area") as Border;
        if (dieArea is null)
        {
            Throw.InvalidUiElementCast(sender, typeof(Border));
        }

        dieArea.BorderBrush = die.IsHold
            ? Brushes.Red
            : Brushes.Chartreuse;
    }

    private void RollBtn_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        _ = _yahtzeeProcessor.RollDice();
        ReplaceDiceImages();
    }

    private void StepBtn_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var clickedBtn = sender as Button;
        if (clickedBtn is null)
        {
            Throw.InvalidUiElementCast(sender, typeof(Button));
        }

        clickedBtn.IsEnabled = false;

        var selectedCombinationName = clickedBtn.Name[..^3];
        var yahtzeeStep = Enum.Parse<YahtzeeStep>(selectedCombinationName);
        var points = _yahtzeeProcessor.Step(yahtzeeStep);

        var label = FindName($"{selectedCombinationName}Lbl") as Label;
        if (label is null)
        {
            Throw.InvalidUiElementCast($"{selectedCombinationName}Lbl", typeof(Label));
        }

        if (selectedCombinationName is "Yahtzee" && points == 50)
        {
            YahtzeeBonusLbl.Content = 100;
        }

        label.Content = points;

        for (var i = 0; i < _diceImages.Count; i++)
        {
            var dieArea = FindName($"DieImg{i}Area") as Border;
            if (dieArea is null)
            {
                Throw.InvalidUiElementCast(sender, typeof(Border));
            }

            dieArea.BorderBrush = Brushes.Chartreuse;
        }

        _yahtzeeProcessor.PcStep();
    }

    private void EnableSecondSectionButtons(bool isEnabled)
    {
        SecondSectionButtons.ForEach(x => (FindName(x) as Button)!.IsEnabled = isEnabled);
    }
        
    private void ReplaceDiceImages()
    {
        for (var i = 0; i < _diceImages.Count; i++)
        {
            var dieImage = _diceImages[i];
            dieImage.Source = new BitmapImage(
                new Uri($"pack://application:,,,/Resources/Die_{_yahtzeeProcessor.Dice[i].Value}.jpeg"));
        }
    }
}