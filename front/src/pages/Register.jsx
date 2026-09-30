import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../api/api";

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

function Register() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errors, setErrors] = useState({});
  const [serverError, setServerError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();

    const nextErrors = {};
    const normalizedEmail = email.trim();

    if (!normalizedEmail) {
      nextErrors.email = "Введите email";
    } else if (!EMAIL_PATTERN.test(normalizedEmail)) {
      nextErrors.email = "Введите корректный email";
    }

    if (!password) {
      nextErrors.password = "Введите пароль";
    }

    if (!confirmPassword) {
      nextErrors.confirmPassword = "Подтвердите пароль";
    } else if (password !== confirmPassword) {
      nextErrors.confirmPassword = "Пароли не совпадают";
    }

    setErrors(nextErrors);
    setServerError("");

    if (Object.keys(nextErrors).length > 0) {
      return;
    }

    setIsLoading(true);

    try {
      await register(normalizedEmail, password);
      navigate("/login", { replace: true, state: { registered: true } });
    } catch (error) {
      setServerError(error.message || "Не удалось зарегистрироваться");
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className="auth-page">
      <section className="auth-card" aria-labelledby="register-title">
        <div className="auth-brand">TODO</div>
        <h1 id="register-title">Регистрация</h1>

        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <div className="auth-field">
            <label className="auth-label" htmlFor="register-email">
              Email
            </label>
            <input
              className="auth-input"
              id="register-email"
              type="email"
              value={email}
              onChange={(event) => {
                setEmail(event.target.value);
                setErrors((current) => ({ ...current, email: "" }));
                setServerError("");
              }}
              autoComplete="email"
              aria-invalid={Boolean(errors.email)}
              aria-describedby={
                errors.email ? "register-email-error" : undefined
              }
            />
            {errors.email && (
              <div className="auth-field-error" id="register-email-error">
                {errors.email}
              </div>
            )}
          </div>

          <div className="auth-field">
            <label className="auth-label" htmlFor="register-password">
              Пароль
            </label>
            <input
              className="auth-input"
              id="register-password"
              type="password"
              value={password}
              onChange={(event) => {
                setPassword(event.target.value);
                setErrors((current) => ({ ...current, password: "" }));
                setServerError("");
              }}
              autoComplete="new-password"
              aria-invalid={Boolean(errors.password)}
              aria-describedby={
                errors.password ? "register-password-error" : undefined
              }
            />
            {errors.password && (
              <div className="auth-field-error" id="register-password-error">
                {errors.password}
              </div>
            )}
          </div>

          <div className="auth-field">
            <label className="auth-label" htmlFor="register-confirm-password">
              Подтвердите пароль
            </label>
            <input
              className="auth-input"
              id="register-confirm-password"
              type="password"
              value={confirmPassword}
              onChange={(event) => {
                setConfirmPassword(event.target.value);
                setErrors((current) => ({
                  ...current,
                  confirmPassword: "",
                }));
                setServerError("");
              }}
              autoComplete="new-password"
              aria-invalid={Boolean(errors.confirmPassword)}
              aria-describedby={
                errors.confirmPassword
                  ? "register-confirm-password-error"
                  : undefined
              }
            />
            {errors.confirmPassword && (
              <div
                className="auth-field-error"
                id="register-confirm-password-error"
              >
                {errors.confirmPassword}
              </div>
            )}
          </div>

          {serverError && (
            <div className="auth-form-error" role="alert">
              {serverError}
            </div>
          )}

          <button className="auth-submit" type="submit" disabled={isLoading}>
            {isLoading ? "Регистрируем..." : "Зарегистрироваться"}
          </button>
        </form>

        <p className="auth-footer">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </section>
    </main>
  );
}

export default Register;
