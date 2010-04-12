/* Code in this file will end up in the generated JScript for all languages */
public function GetSafe(p, i : int) : String
{
	return p.Length > i ? p[i] : "<Param Missing>";
}