import type {
  DeleteTaskResponse,
  LoginResponse,
  RegisterResponse,
  Todo,
} from "../types/todo";

const API_URL = "";

type RequestOptions = Omit<RequestInit, "headers"> & {
  headers?: Record<string, string>;
};

function withAuthorization(options: RequestOptions = {}): RequestOptions {
  const token = localStorage.getItem("token");

  return {
    ...options,
    headers: {
      ...options.headers,
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
  };
}

async function request<T>(
  path: string,
  options: RequestInit = {},
  errorMessage: string,
): Promise<T> {
  let response: Response;

  try {
    response = await fetch(`${API_URL}${path}`, options);
  } catch {
    throw new Error("Не удалось подключиться к серверу");
  }

  const responseText = await response.text();
  let data: unknown = null;

  if (responseText) {
    try {
      data = JSON.parse(responseText);
    } catch {
      data = responseText;
    }
  }

  if (!response.ok) {
    const serverMessage =
      typeof data === "string"
        ? data
        : typeof data === "object" &&
            data !== null &&
            "message" in data &&
            typeof data.message === "string"
          ? data.message
          : undefined;

    throw new Error(serverMessage || errorMessage);
  }

  return data as T;
}

export async function login(
  email: string,
  password: string,
): Promise<LoginResponse> {
  return request(
    "/api/auth/login",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    },
    "Не удалось войти",
  );
}

export async function register(
  email: string,
  password: string,
): Promise<RegisterResponse> {
  return request(
    "/api/auth/register",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    },
    "Не удалось зарегистрироваться",
  );
}

export async function getTasks(): Promise<Todo[]> {
  return request(
    "/api/tasks",
    withAuthorization(),
    "Не удалось загрузить задачи",
  );
}

export async function createTask(
  title: string,
  description: string,
): Promise<Todo> {
  return request(
    "/api/tasks",
    withAuthorization({
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ title, description, completed: false }),
    }),
    "Не удалось создать задачу",
  );
}

export async function updateTask(id: number, title: string, description: string, completed: boolean): Promise<Todo> {
  return request(
    `/api/tasks/${id}`,
    withAuthorization({
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ title, description, completed }),
    }),
    "Не удалось изменить задачу",
  );
}

export async function deleteTask(id: number): Promise<DeleteTaskResponse> {
  return request(
    `/api/tasks/${id}`,
    withAuthorization({ method: "DELETE" }),
    "Не удалось удалить задачу",
  );
}
