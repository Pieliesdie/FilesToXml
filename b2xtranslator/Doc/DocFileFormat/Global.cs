namespace b2xtranslator.doc.DocFileFormat;

public class Global
{
    public enum CellWidthType
    {
        nil,
        auto,
        pct,
        dxa
    }
    
    public enum ColorIdentifier
    {
        auto = 0,
        black,
        blue,
        cyan,
        green,
        magenta,
        red,
        yellow,
        white,
        darkBlue,
        darkCyan,
        darkGreen,
        darkMagenta,
        darkRed,
        darkYellow,
        darkGray,
        lightGray
    }
    
    public enum DashStyle
    {
        solid,
        shortdash,
        shortdot,
        shortdashdot,
        shortdashdotdot,
        dot,
        dash,
        longdash,
        dashdot,
        longdashdot,
        longdashdotdot
    }
    
    public enum FarEastLayout
    {
        none,
        tatenakayoko,
        warichu,
        kumimoji,
        all
    }
    
    public enum HorizontalPositionCode
    {
        text = 0,
        margin,
        page,
        none
    }
    
    public enum HyphenationRule
    {
        none,
        normal,
        addLetterBefore,
        changeLetterBefore,
        deleteLetterBefore,
        changeLetterAfter,
        deleteAndChange
    }
    
    public enum JustificationCode
    {
        left = 0,
        center,
        right,
        both,
        distribute,
        mediumKashida,
        numTab,
        highKashida,
        lowKashida,
        thaiDistribute
    }
    
    public enum TabLeader
    {
        none = 0,
        dot,
        hyphen,
        underscore,
        heavy,
        middleDot
    }
    
    public enum TextAnimation
    {
        none,
        lights,
        blinkBackground,
        sparkle,
        antsBlack,
        antsRed,
        shimmer
    }
    
    public enum TextFlow
    {
        lrTb = 0,
        tbRl = 1,
        btLr = 3,
        lrTbV = 4,
        tbRlV = 5
    }
    
    public enum TextFrameWrapping
    {
        auto,
        notBeside,
        around,
        none,
        tight,
        through
    }
    
    public enum UnderlineCode
    {
        none = 0,
        single,
        word,
        Double,
        dotted,
        notUsed1,
        thick,
        dash,
        notUsed2,
        dotDash,
        dotDotDash,
        wave,
        dottedHeavy,
        dashedHeavy,
        dashDotHeavy,
        dashDotDotHeavy,
        wavyHeavy,
        dashLong,
        wavyDouble,
        dashLongHeavy
    }
    
    public enum VerticalAlign
    {
        top,
        center,
        bottom
    }
    
    public enum VerticalMergeFlag
    {
        fvmClear = 0,
        fvmMerge = 1,
        fvmRestart = 3
    }
    
    public enum VerticalPositionCode
    {
        margin = 0,
        page,
        text,
        none
    }
    
    public enum WarichuBracket
    {
        none,
        parentheses,
        squareBrackets,
        angledBrackets,
        braces
    }
}