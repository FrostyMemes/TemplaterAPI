import './App.css';
import {useState, useEffect} from "react";
import axios from "axios";
import useDebounce from "./use-debounce";

function App() {
    const apiUrl = `${process.env.REACT_APP_BACKEND_ADDRESS}/api`
    const [markdown, setMarkdown] = useState('')
    const [mdRender, setMdRender] = useState('')

    const debouncedSearchTerm = useDebounce(markdown, 700);
    function sendMarkdown(value) {
        axios.get(`http://${apiUrl}?markdown=${value}`)
            .then(response => setMdRender(response.data))
            .catch(error => {
                setMdRender(error.message)
            });
    }

    useEffect(
        () => {
            if (debouncedSearchTerm) {
                sendMarkdown(debouncedSearchTerm)
            } else {
                setMdRender('');
            }
        },
        [debouncedSearchTerm]
    );

    return (
        <div>
            <div className="work-space">
                <div className="input-area">
                    <textarea placeholder={apiUrl}
                              onChange={(e) => setMarkdown(e.target.value)}>
                    </textarea>
                </div>
                <div className="render-area" dangerouslySetInnerHTML={{__html: mdRender}}></div>
            </div>
            <input type="button" value="Отправить" onClick={sendMarkdown}/>
        </div>
    );
}

export default App;
