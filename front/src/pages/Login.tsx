import { useState } from "react";
import type { FormEvent } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { login } from "../api/api";

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

interface LoginErrors {
  email?: string;
  password?: string;
}

interface LoginLocationState {
  registered?: boolean;
}

function Login() {
  const navigate = useNavigate();
  const locationState = useLocation().state as LoginLocationState | null;
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<LoginErrors>({});
  const [serverError, setServerError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const nextErrors: LoginErrors = {};
    const normalizedEmail = email.trim();

    if (!normalizedEmail) {
      nextErrors.email = "Введите email";
    } else if (!EMAIL_PATTERN.test(normalizedEmail)) {
      nextErrors.email = "Введите корректный email";
    }

    if (!password) {
      nextErrors.password = "Введите пароль";
    }

    setErrors(nextErrors);
    setServerError("");

    if (Object.keys(nextErrors).length > 0) {
      return;
    }

    setIsLoading(true);

    try {
      const data = await login(normalizedEmail, password);

      if (!data.token) {
        throw new Error("Сервер не вернул token");
      }

      localStorage.setItem("token", data.token);
      navigate("/", { replace: true });
    } catch (error: unknown) {
      setServerError(
        error instanceof Error ? error.message : "Не удалось войти",
      );
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className="auth-page">
      <section className="auth-card" aria-labelledby="login-title">
        <div className="auth-brand">TODO</div>
        <h1 id="login-title">Вход</h1>

        {locationState?.registered && (
          <div className="auth-success" role="status">
            Аккаунт создан. Теперь войдите.
          </div>
        )}

        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <div className="auth-field">
            <label className="auth-label" htmlFor="login-email">
              Email
            </label>
            <input
              className="auth-input"
              id="login-email"
              type="email"
              value={email}
              onChange={(event) => {
                setEmail(event.target.value);
                setErrors((current) => ({ ...current, email: "" }));
                setServerError("");
              }}
              autoComplete="email"
              aria-invalid={Boolean(errors.email)}
              aria-describedby={errors.email ? "login-email-error" : undefined}
            />
            {errors.email && (
              <div className="auth-field-error" id="login-email-error">
                {errors.email}
              </div>
            )}
          </div>

          <div className="auth-field">
            <label className="auth-label" htmlFor="login-password">
              Пароль
            </label>
            <input
              className="auth-input"
              id="login-password"
              type="password"
              value={password}
              onChange={(event) => {
                setPassword(event.target.value);
                setErrors((current) => ({ ...current, password: "" }));
                setServerError("");
              }}
              autoComplete="current-password"
              aria-invalid={Boolean(errors.password)}
              aria-describedby={
                errors.password ? "login-password-error" : undefined
              }
            />
            {errors.password && (
              <div className="auth-field-error" id="login-password-error">
                {errors.password}
              </div>
            )}
          </div>

          {serverError && (
            <div className="auth-form-error" role="alert">
              {serverError}
            </div>
          )}

          <button className="auth-submit" type="submit" disabled={isLoading}>
            {isLoading ? "Входим..." : "Войти"}
          </button>
        </form>

        <p className="auth-footer">
          Нет аккаунта? <Link to="/register">Зарегистрироваться</Link>
        </p>
      </section>
    </main>
  );
}

export default Login;
