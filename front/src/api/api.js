const API_URL = "http://localhost:5177";

async function request(path, options, errorMessage) {
  const response = await fetch(`${API_URL}${path}`, options);

  if (!response.ok) {
    throw new Error(errorMessage);
  }

  return response.json();
}

export async function getTasks() {
  return request("/api/tasks", undefined, "Не удалось загрузить задачи");
}

export async function createTask(title, description) {
  return request(
    "/api/tasks",
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ title, description, completed: false }),
    },
    "Не удалось создать задачу",
  );
}

export async function updateTask(id, title, description, completed) {
  return request(
    `/api/tasks/${id}`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ title, description, completed }),
    },
    "Не удалось изменить задачу",
  );
}

export async function deleteTask(id) {
  return request(
    `/api/tasks/${id}`,
    { method: "DELETE" },
    "Не удалось удалить задачу",
  );
}
