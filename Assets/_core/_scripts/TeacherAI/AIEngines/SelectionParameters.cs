using System.Collections.Generic;
using Antura.Database;

namespace Antura.Teacher
{
    /// <summary>
    /// Defines how to handle repetition when selecting learning data.
    /// </summary>
    public enum SelectionSeverity
    {
        AsManyAsPossible,       // If possible, the given number of data values is asked for, or less if there are not enough.
        AllRequired,            // The given number of data values is required. Error if it is not reached.
        MayRepeatIfNotEnough    // May repeat the same values if not enough values are found
    }

    /// <summary>
    /// Define how to handle multiple question packs.
    /// </summary>
    public enum PackListHistory
    {
        NoFilter,               // Multiple packs in the game have no influence one over the other
        ForceAllDifferent,      // Make sure that multiple packs in a list do not contain the same values
        RepeatWhenFull,         // Try to make sure that multiple packs have not the same values, fallback to NoFilter if we cannot get enough data
        SkipPacks,              // If we cannot find enough data, reduce the number of packs to be generated
    }

    /// <summary>
    /// Parameters for filtering and selecting learning data based on the minigame requirements, used by QuestionBuilders.
    /// </summary>
    public struct SelectionParameters
    {
        public enum JourneyFilter
        {
            CurrentJourney,                                  // Can use data up to the current Journey Position
            UpToFullCurrentStage,                            // Can use data up to the current Stage
            CurrentLearningBlockOnly,                        // Can only use data from the current learning block
            CurrentLearningBlockFallbackToCurrentJourney,   // Can only use data from the current learning block. If a question pack cannot be created with such data, we fallback to the current journey.
        }

        public enum PriorityFilter
        {
            NoPriority,             // Data is not prioritized at all
            CurrentThenExpand,      // If the current PS has not enough data, try the current LB, then the current Stage
            CurrentThenPast         // If the current PS has not enough data, go to previous PS
        }

        public SelectionSeverity severity;
        public int nRequired;
        public bool getMaxData;
        public bool useJourney;
        public JourneyFilter journeyFilter;
        public PriorityFilter priorityFilter;

        public PackListHistory packListHistory;
        public List<string> filteringIds;
        public bool sortDataByDifficulty;

        public SelectionParameters(SelectionSeverity severity, int nRequired = 0, bool getMaxData = false, bool useJourney = true,
            JourneyFilter journeyFilter = JourneyFilter.CurrentJourney, PackListHistory packListHistory = PackListHistory.NoFilter, List<string> filteringIds = null, bool sortDataByDifficulty = false,
            PriorityFilter priorityFilter = PriorityFilter.CurrentThenPast)
        {
            this.nRequired = nRequired;
            this.getMaxData = getMaxData;
            this.severity = severity;
            this.useJourney = useJourney;
            this.journeyFilter = journeyFilter;
            this.packListHistory = packListHistory;
            this.filteringIds = filteringIds;
            this.sortDataByDifficulty = sortDataByDifficulty;
            this.priorityFilter = priorityFilter;
        }

        public void AssignJourney(bool insideJourney)
        {
            journeyFilter = JourneyFilter.CurrentLearningBlockFallbackToCurrentJourney;
            priorityFilter = insideJourney ? PriorityFilter.CurrentThenPast : PriorityFilter.NoPriority;
        }
    }

    /// <summary>
    /// Parameters used to configure a QuestionBuilder.
    /// </summary>
    public class QuestionBuilderParameters
    {
        public PackListHistory correctChoicesHistory;
        public PackListHistory wrongChoicesHistory; // Always set as NoFilter for now.
        public bool useJourneyForWrong;
        public bool useJourneyForCorrect;
        public SelectionSeverity correctSeverity;
        public SelectionSeverity wrongSeverity;
        public bool sortPacksByDifficulty;
        public bool insideJourney;

        // data-based params
        public LetterEqualityStrictness letterEqualityStrictness; // Strictness to assign to letters for equality, when generated by this builder
        public LetterFilters letterFilters;
        public WordFilters wordFilters;
        public PhraseFilters phraseFilters;

        public QuestionBuilderParameters()
        {
            this.correctChoicesHistory = PackListHistory.RepeatWhenFull;
            this.wrongChoicesHistory = PackListHistory.RepeatWhenFull;
            this.useJourneyForCorrect = true;
            this.useJourneyForWrong = true;
            this.correctSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            this.wrongSeverity = SelectionSeverity.MayRepeatIfNotEnough;
            this.letterEqualityStrictness = LetterEqualityStrictness.Letter;
            this.letterFilters = new LetterFilters();
            this.wordFilters = new WordFilters();
            this.phraseFilters = new PhraseFilters();
            this.sortPacksByDifficulty = true;
            this.insideJourney = true;
        }

        public SelectionParameters.JourneyFilter JourneyFilter => SelectionParameters.JourneyFilter.CurrentJourney;
    }
}
