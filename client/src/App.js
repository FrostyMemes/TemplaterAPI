import './App.css';

function App() {
    console.log(process.env.REACT_APP_BACKEND_IP)
    return (
        <div>
            <div className="render-space">
                <div className="input-area">
                    <textarea placeholder={process.env.REACT_APP_BACKEND_IP}></textarea>
                </div>
                <div className="render-area">
                    Вывод
                </div>
            </div>
            <input type="button" value="Отправить"/>
        </div>
    );
}

export default App;
