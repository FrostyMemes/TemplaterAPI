import './App.css';
import EditorMarkdown from './components/editor/EditorMarkdown'


function App() {

    return (
        <div>
            <EditorMarkdown title="Example"/>
            <input type="button" value="Save to DataBase"/>
        </div>
    );
}

export default App;
