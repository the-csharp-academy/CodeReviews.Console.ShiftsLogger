// API/apiService.js

export class ApiService {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  async request(endpoint, options = {}) {
    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, options);
      const text = await response.text(); // handle both JSON and text cases
      const contentType = response.headers.get("content-type");
      const result = contentType?.includes("application/json")
        ? JSON.parse(text)
        : text;

      if (!response.ok) {
        // server returned 400–500 range
        throw {
          status: response.status,
          message: result.message || "Server error occurred.",
          details: result.details || result,
        };
      }

      return { status: response.status, data: result };
    } catch (error) {
      // network or server-level issues
      throw {
        status: error.status || 0,
        message: error.message || "Network error",
        details: error.details || error,
      };
    }
  }

  async get(endpoint) {
    return this.request(endpoint, {
      method: "GET",
      headers: { "Content-Type": "application/json" }
    });
  }

  async getValue(endpoint, data) {
    return this.request(`${endpoint}?employeeId=${data.employeeId}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" }
    });
  }

  async post(endpoint, data) {
    return this.request(endpoint, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data)
    });
  }
}
