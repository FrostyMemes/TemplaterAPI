import './App.css';
import {useState} from "react";
import axios from "axios";

function App() {
    const apiUrl = `${process.env.REACT_APP_BACKEND_ADDRESS}/api`
    const [markdown, setMarkdown] = useState('')
    const [mdRender, setMdrender] = useState('')

    const sendMarkdown = function (){
        axios.get(`http://${apiUrl}?markdown=${markdown}`)
            .then(response => setMdrender(response.data))
            .catch(error => {
                setMdrender(error.message)
            });
        //setMarkdown(`${apiUrl}?markdown=${markdown}`)
    }

    return (
        <div>
            <div className="render-space">
                <div className="input-area">
                    <textarea placeholder={apiUrl}
                              onChange={(e) => setMarkdown(e.target.value)}>
                    </textarea>
                </div>
                <div className="render-area">
                    {mdRender}
                </div>
            </div>
            <input type="button" value="Отправить" onClick={sendMarkdown}/>
        </div>
    );
}

export default App;
