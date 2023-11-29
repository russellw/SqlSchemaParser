namespace SqlSchemaParser;
// Is RESTRICT a synonym of NO ACTION? It seems the answer is 'almost'
// https://stackoverflow.com/questions/14921668/difference-between-restrict-and-no-action
// For now, treat them as synonyms
public enum Action {
	NO_ACTION,
	CASCADE,
	SET_NULL,
	SET_DEFAULT,
}
