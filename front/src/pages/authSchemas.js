import { z } from "zod";

const emailSchema = z
  .string()
  .trim()
  .min(1, "Введите email")
  .email("Введите корректный email")
  .max(256, "Email слишком длинный");

const passwordSchema = z
  .string()
  .min(1, "Введите пароль")
  .min(8, "Пароль должен содержать минимум 8 символов")
  .max(128, "Пароль не должен превышать 128 символов");

export const loginSchema = z.object({
  email: emailSchema,
  password: passwordSchema,
});

export const registerSchema = z
  .object({
    email: emailSchema,
    password: passwordSchema,
    confirmPassword: z.string().min(1, "Подтвердите пароль"),
  })
  .refine((values) => values.password === values.confirmPassword, {
    path: ["confirmPassword"],
    message: "Пароли не совпадают",
  });

export function getFieldErrors(error) {
  return error.issues.reduce((fieldErrors, issue) => {
    const field = issue.path[0];

    if (typeof field === "string" && !fieldErrors[field]) {
      fieldErrors[field] = issue.message;
    }

    return fieldErrors;
  }, {});
}
