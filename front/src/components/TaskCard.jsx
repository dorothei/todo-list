function TaskCard({ task, selected, onSelect, onToggle }) {
  function handleToggle(event) {
    event.stopPropagation();

    onToggle(task);
  }

  return (
    <div
      className={`task-row ${selected ? "selected" : ""}`}
      onClick={() => onSelect(task)}
    >
      <button
        className={`task-checkbox ${task.completed ? "checked" : ""}`}
        onClick={handleToggle}
        aria-label="Изменить статус задачи"
      >
        {task.completed ? "✓" : ""}
      </button>

      <div className="task-row-content">
        <div className={`task-row-title ${task.completed ? "completed" : ""}`}>
          {task.title}
        </div>

        {task.description && (
          <div className="task-row-description">{task.description}</div>
        )}
      </div>
    </div>
  );
}

export default TaskCard;
