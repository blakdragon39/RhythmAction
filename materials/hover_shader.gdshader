shader_type spatial;

instance uniform bool isHighlighted = false;
const vec3 color = vec3(12.0/255.0, 142.0/255.0, 19.0/255.0);

void fragment() {
	if (isHighlighted) {
		ALBEDO = vec3(color.r + 0.1, color.g + 0.1, color.b + 0.1);
	} else {
		ALBEDO = color;
	}

}
