const ENDPOINTS = {
  AUTH: {
    LOGIN: "/auth/login",
  },
  DOCTOR_DASHBOARD: {
    GET: "/doctor/dashboard",
    TODAY_APPOINTMENTS: "/doctor/dashboard/today-appointments",
  },
  HOSPITAL: {
    BRANCHES: {
      LIST: "/hospital/branches",
      CREATE: "/hospital/branches",
      GET_BY_ID: (id) => `/hospital/branches/${id}`,
      UPDATE: (id) => `/hospital/branches/${id}`,
      DELETE: (id) => `/hospital/branches/${id}`,
      ACTIVATE: (id) => `/hospital/branches/${id}/activate`,
      DEACTIVATE: (id) => `/hospital/branches/${id}/deactivate`,
    },
    DEPARTMENTS: {
      LIST: "/hospital/departments",
      CREATE: "/hospital/departments",
      GET_BY_ID: (id) => `/hospital/departments/${id}`,
      UPDATE: (id) => `/hospital/departments/${id}`,
      DELETE: (id) => `/hospital/departments/${id}`,
      ACTIVATE: (id) => `/hospital/departments/${id}/activate`,
      DEACTIVATE: (id) => `/hospital/departments/${id}/deactivate`,
    },
    DOCTORS: {
      LIST: "/hospital/doctors",
      CREATE: "/hospital/doctors",
      GET_BY_ID: (id) => `/hospital/doctors/${id}`,
      UPDATE: (id) => `/hospital/doctors/${id}`,
      DELETE: (id) => `/hospital/doctors/${id}`,
      ACTIVATE: (id) => `/hospital/doctors/${id}/activate`,
      DEACTIVATE: (id) => `/hospital/doctors/${id}/deactivate`,
      RESET_PASSWORD: (id) => `/hospital/doctors/${id}/reset-password`,
    },
    DOCTOR_DEPARTMENTS: {
      LIST: (doctorId) => `/hospital/doctors/${doctorId}/departments`,
      ASSIGN: (doctorId) => `/hospital/doctors/${doctorId}/departments`,
      REMOVE: (doctorId, departmentId) =>
        `/hospital/doctors/${doctorId}/departments/${departmentId}`,
    },
    ROLES: {
      LIST: "/hospital/roles",
    },
    STAFF: {
      LIST: "/hospital/staff",
      CREATE: "/hospital/staff",
      GET_BY_ID: (id) => `/hospital/staff/${id}`,
      UPDATE: (id) => `/hospital/staff/${id}`,
      DELETE: (id) => `/hospital/staff/${id}`,
      ACTIVATE: (id) => `/hospital/staff/${id}/activate`,
      DEACTIVATE: (id) => `/hospital/staff/${id}/deactivate`,
    },
    PROFILE: {
      GET: "/hospital/profile",
      UPDATE: "/hospital/profile",
    },
  },
  APPOINTMENTS: {
    LIST: "/hospital/appointments",
  },
};

export default ENDPOINTS;