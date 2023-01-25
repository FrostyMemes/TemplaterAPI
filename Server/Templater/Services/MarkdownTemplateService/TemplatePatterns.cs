using System.Text.RegularExpressions;
using Templater.Patterns;
using Templater.Patterns.Combinators;

namespace Templater.Builder;

public class TemplatePatterns
{
    static readonly PatternResult voidResult = new (null, -1);
    
    public static readonly Regex ptrMarkGroupWords = new (
        @"""([^""\\]*(\\.[^""\\]*)*)""|\'([^\'\\]*(\\.[^\'\\]*)*)\'");
    
    public static readonly AlternativePattern ptrSquareBraceArea = new (
        new RegexPattern(
            new Regex(@"\[(.*)\]")), voidResult);

    public static readonly AlternativePattern ptrRoundBraceArea = new (
        new RegexPattern(
            new Regex(@"\((.*)\)")), voidResult);

    public static readonly AlternativePattern ptrVerticalBraceArea = new (
        new RegexPattern(
            new Regex(@"\|(.*)\|")), voidResult);

    public static readonly AlternativePattern ptrFigureBraceArea = new (
        new RegexPattern(
            new Regex(@"\{(.*)\}")), voidResult);
    
    public static readonly AlternativePattern ptrSingleMarkArea = new (
        new RegexPattern(
            new Regex(@"\'(.*)\'")), voidResult);
    
    public static readonly AlternativePattern ptrDuoMarkArea = new (
        new RegexPattern(
            new Regex(@"\""(.*)\""")), voidResult);

    public static readonly AlternativePattern ptrRoundBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\()(.*?)(?=\))")), voidResult);

    public static readonly AlternativePattern ptrSquareBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\[)(.*?)(?=\])")), voidResult);

    public static readonly AlternativePattern ptrVerticalBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\|)(.*?)(?=\|)")), voidResult);

    public static readonly AlternativePattern ptrFigureBraceContent = new (
        new RegexPattern(
            new Regex(@"(?<=\{)(.*?)(?=\})")), voidResult);

    public static readonly AlternativePattern ptrSingleMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\')(.*?)(?=\')")), voidResult);

    public static readonly AlternativePattern ptrDuoMarkContent = new (
        new RegexPattern(
            new Regex(@"(?<=\"")(.*?)(?=\"")")), voidResult);
    
    public static readonly AlternativePattern ptrMarksArea = new (
        new AnyPattern(ptrSingleMarkArea, ptrDuoMarkArea), voidResult);
    
    public static readonly AlternativePattern ptrMarksContent = new (
        new AnyPattern(ptrSingleMarkContent, ptrDuoMarkContent), voidResult);
    
    public static readonly Dictionary<string, Pattern[]> ptrEnumTags = new ()
    {
        {"radio", new []{ptrRoundBraceArea, ptrRoundBraceContent}},
        {"checkbox", new []{ptrSquareBraceArea, ptrSquareBraceContent}}
    };
    
}