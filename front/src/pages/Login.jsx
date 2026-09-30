import { Link } from "react-router-dom";

function Login() {
  function handleSubmit(event) {
    event.preventDefault();
  }

  return (
    <div>
      <h1>Вход</h1>

      <form onSubmit={handleSubmit}>
        <input
          type="email"
          placeholder="Email"
        />

        <input
          type="password"
          placeholder="Пароль"
        />

        <button type="submit">Войти</button>
      </form>

      <p>
        Нет аккаунта? <Link to="/register">Зарегистрироваться</Link>
      </p>
    </div>
  );
}

export default Login;
