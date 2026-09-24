const ENDPOINTS = {
  AUTH: {
    LOGIN: "/auth/login",
  },
  DOCTOR_DASHBOARD: {
    GET: "/doctor/dashboard",
    TODAY_APPOINTMENTS: "/doctor/dashboard/today-appointments",
  },
  HOSPITAL: {
    BRANCHES: "/hospital/branches",
    DEPARTMENTS: "/hospital/departments",
    DOCTORS: "/hospital/doctors",
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